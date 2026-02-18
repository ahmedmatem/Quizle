using Microsoft.EntityFrameworkCore;
using Quizle.Core.Contracts;
using Quizle.Core.Dtos;
using Quizle.Core.Entities;
using Quizle.Core.Types;

namespace Quizle.Core.Services
{
    public class TeacherQuestionService : ITeacherQuestionService
    {
        private readonly IQuestionRepository _questionRepository;

        public TeacherQuestionService(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public async Task<string> CreateAsync(string teacherId, QuestionEditDto dto, CancellationToken ct)
        {
            Validate(dto);

            var q = new Question
            {
                Id = Guid.NewGuid().ToString(),
                CreatedByUserId = teacherId,
                Text = dto.Text.Trim(),
                Points = dto.Points,
                Type = dto.Type,
                CorrectNumeric = dto.Type == QuestionType.Numeric ? dto.CorrectNumeric : null,
                NumericTolerance = dto.Type == QuestionType.Numeric ? dto.NumericTolerance : null,

                // IMPORTANT: do NOT set CorrectOptionId here
                CorrectOptionId = null
            };

            await _questionRepository.AddAsync(q, ct);
            await _questionRepository.SaveChangesAsync(ct); // 1) Question exists

            if (dto.Type == QuestionType.MultipleChoice)
            {
                var options = dto.Options
                    .Where(o => !string.IsNullOrWhiteSpace(o.Text))
                    .Select(o => new ChoiceOption
                    {
                        Id = Guid.NewGuid().ToString(),
                        QuestionId = q.Id,
                        Text = o.Text.Trim()
                    })
                    .ToList();

                if (options.Count < 2)
                    throw new InvalidOperationException("Multiple choice must have at least 2 options.");

                if (!dto.CorrectIndex.HasValue || dto.CorrectIndex < 0 || dto.CorrectIndex >= options.Count)
                    throw new InvalidOperationException("Choose correct option.");

                await _questionRepository.AddOptionsAsync(options, ct);
                await _questionRepository.SaveChangesAsync(ct); // 2) Options exist

                // 3) set correct FK after options exist
                q.CorrectOptionId = options[dto.CorrectIndex.Value].Id;
                await _questionRepository.SaveChangesAsync(ct);
            }

            return q.Id;
        }

        public async Task<QuestionEditDto> GetForEditAsync(string teacherId, string id, CancellationToken ct)
        {
            var q = await _questionRepository.GetIncludeOptionsAsync(id, teacherId, ct);

            if (q == null) throw new InvalidOperationException("Question not found or access denied.");

            var dto = new QuestionEditDto
            {
                Id = q.Id,
                Text = q.Text,
                Points = q.Points,
                Type = q.Type,
                CorrectNumeric = q.CorrectNumeric,
                NumericTolerance = q.NumericTolerance,
                Options = q.Options
                .OrderBy(o => o.Id)
                .Select(o => new OptionInputDto { Id = o.Id, Text = o.Text })
                .ToList()
            };

            if (q.Type == QuestionType.MultipleChoice && q.Options.Count > 0 && q.CorrectOptionId != null)
            {
                var idx = dto.Options.FindIndex(o => o.Id == q.CorrectOptionId);
                dto.CorrectIndex = idx >= 0 ? idx : null;
            }

            return dto;
        }

        public async Task<QuestionIndexDto> GetIndexAsync(string teacherId, string? search, QuestionType? type, CancellationToken ct)
        {
            var q = _questionRepository.GetByCreator(teacherId);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();
                q = q.Where(q => q.Text.Contains(s));
            }

            if (type.HasValue)
                q = q.Where(q => q.Type == type.Value);

            var items = await q
            .OrderByDescending(x => x.Id)
            .Select(x => new QuestionListItemDto
            {
                Id = x.Id,
                Text = x.Text,
                Type = x.Type,
                Points = x.Points,
                OptionsCount = x.Options.Count
            })
            .ToListAsync(ct);

            return new QuestionIndexDto { Search = search, Type = type, Items = items };
        }

        private static void Validate(QuestionEditDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Text))
                throw new InvalidOperationException("Question text is required.");

            if (dto.Points < 1 || dto.Points > 100)
                throw new InvalidOperationException("Points must be between 1 and 100.");

            if (dto.Type == QuestionType.Numeric)
            {
                if (!dto.CorrectNumeric.HasValue)
                    throw new InvalidOperationException("Correct numeric value is required.");
                if (dto.NumericTolerance.HasValue && dto.NumericTolerance < 0)
                    throw new InvalidOperationException("Tolerance cannot be negative.");
            }

            if (dto.Type == QuestionType.MultipleChoice)
            {
                dto.Options = dto.Options
                    .Where(o => !string.IsNullOrWhiteSpace(o.Text))
                    .ToList();

                if (dto.Options.Count < 2)
                    throw new InvalidOperationException("Multiple choice must have at least 2 options.");

                if (!dto.CorrectIndex.HasValue || dto.CorrectIndex < 0 || dto.CorrectIndex >= dto.Options.Count)
                    throw new InvalidOperationException("Choose correct option.");
            }
        }

        public async Task UpdateAsync(string teacherId, QuestionEditDto dto, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(dto.Id))
                throw new InvalidOperationException("Invalid id.");

            Validate(dto);

            var q = await _questionRepository.GetIncludeOptionsAsync(dto.Id, teacherId, ct);

            if (q == null)
                throw new InvalidOperationException("Question not found or access denied.");

            q.Text = dto.Text.Trim();
            q.Points = dto.Points;
            q.Type = dto.Type;

            // reset type-specific
            q.CorrectNumeric = dto.Type == QuestionType.Numeric ? dto.CorrectNumeric : null;
            q.NumericTolerance = dto.Type == QuestionType.Numeric ? dto.NumericTolerance : null;

            if (dto.Type != QuestionType.MultipleChoice)
            {
                q.CorrectOptionId = null;
                // optional: soft-delete all options when changing away from MC
                // foreach (var opt in q.Options.ToList()) _questionRepository.RemoveChoiceOption(opt);
                await _questionRepository.SaveChangesAsync(ct);
                return;
            }

            // ----- MULTIPLE CHOICE SYNC -----

            // normalize incoming
            var incoming = dto.Options
                .Where(o => !string.IsNullOrWhiteSpace(o.Text))
                .Select(o => new
                {
                    Id = string.IsNullOrWhiteSpace(o.Id) ? null : o.Id.Trim(),
                    Text = o.Text.Trim()
                })
                .ToList();

            if (incoming.Count < 2)
                throw new InvalidOperationException("Multiple choice must have at least 2 options.");

            if (!dto.CorrectIndex.HasValue || dto.CorrectIndex < 0 || dto.CorrectIndex >= incoming.Count)
                throw new InvalidOperationException("Choose correct option.");

            // 1) remove missing existing options
            var incomingIds = incoming.Where(x => x.Id != null).Select(x => x.Id!).ToHashSet();

            var toRemove = q.Options.Where(o => !incomingIds.Contains(o.Id)).ToList();
            foreach (var r in toRemove)
            {
                q.Options.Remove(r);                        // detach from nav
                _questionRepository.RemoveChoiceOption(r);  // soft delete interceptor will run
            }

            // 2) update existing + add new (keep an aligned list in vm order)
            var aligned = new List<ChoiceOption>();

            foreach (var inc in incoming)
            {
                if (inc.Id != null)
                {
                    var existing = q.Options.FirstOrDefault(o => o.Id == inc.Id);
                    if (existing == null)
                        throw new InvalidOperationException("Option not found (stale form).");

                    existing.Text = inc.Text;
                    aligned.Add(existing);
                }
                else
                {
                    var created = new ChoiceOption
                    {
                        Id = Guid.NewGuid().ToString(),
                        QuestionId = q.Id,
                        Text = inc.Text
                    };

                    q.Options.Add(created);     // add via nav
                                                // _questionRepository.AddChoiceOption(created); // optional if you prefer explicit add
                    aligned.Add(created);
                }
            }

            // 3) set correct option by index directly from aligned list (no requery needed)
            q.CorrectOptionId = aligned[dto.CorrectIndex.Value].Id;

            await _questionRepository.SaveChangesAsync(ct);
        }

        public async Task SoftDeleteAsync(string teacherId, string id, CancellationToken ct)
        {
            var q = await _questionRepository.GetAsync(id,teacherId, ct);
            
            if (q == null) return;

            _questionRepository.Remove(q); // intercepted by soft delete
            await _questionRepository.SaveChangesAsync(ct);
        }
    }
}
