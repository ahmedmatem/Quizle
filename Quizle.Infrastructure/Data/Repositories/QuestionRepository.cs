using Microsoft.EntityFrameworkCore;
using Quizle.Core.Contracts;
using Quizle.Core.Dtos;
using Quizle.Core.Entities;
using Quizle.Core.Types;
using static Quizle.Core.AppConstants;

namespace Quizle.Infrastructure.Data.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly ApplicationDbContext _db;

        public QuestionRepository(ApplicationDbContext db) => _db = db;

        public Task<List<string>> GetOrderedQuestionsAsync(string quizId, CancellationToken ct)
            => _db.QuizQuestions
            .Where(qq => qq.QuizId == quizId)
            .OrderBy(qq => qq.Order)
            .Select(qq => qq.QuestionId)
            .ToListAsync(ct);

        public Task<Question> GetWithOptionsAsync(string questionId, CancellationToken ct)
            => _db.Questions.Include(q => q.Options).FirstAsync(q => q.Id == questionId, ct);

        public Task<bool> BelongsToQuizAsync(string quizId, string questionId, CancellationToken ct)
            => _db.QuizQuestions.AnyAsync(qq => qq.QuizId == quizId && qq.QuestionId == questionId, ct);

        public Task<Dictionary<string, Question>> GetAllAsync(List<string> questionIds, CancellationToken ct)
            => _db.Questions.Where(q => questionIds.Contains(q.Id))
                .ToDictionaryAsync(q => q.Id, ct);

        public IQueryable<Question> GetByCreator(string creatorId)
            => _db.Questions.AsNoTracking().Where(q => q.CreatedByUserId == creatorId);

        public Task<Question?> GetIncludeOptionsAsync(string qId, string creatorId, CancellationToken ct)
            => _db.Questions.Include(q => q.Options).FirstOrDefaultAsync(q => q.Id == qId && q.CreatedByUserId == creatorId, ct);

        public Task AddAsync(Question question, CancellationToken ct)
        => _db.Questions.AddAsync(question, ct).AsTask();

        public Task AddOptionsAsync(IEnumerable<ChoiceOption> options, CancellationToken ct)
            => _db.ChoiceOptions.AddRangeAsync(options, ct);

        public void RemoveChoiceOption(ChoiceOption option) => _db.ChoiceOptions.Remove(option);

        public void AddChoiceOption(ChoiceOption option) => _db.ChoiceOptions.Add(option);

        public Task SaveChangesAsync(CancellationToken ct)
            => _db.SaveChangesAsync(ct);

        public Task<Question?> GetAsync(string questionId, string creatorId, CancellationToken ct)
            => _db.Questions.FirstOrDefaultAsync(q => q.Id == questionId && q.CreatedByUserId == creatorId, ct);

        public void Remove(Question question) => _db.Remove(question);

        public Task<List<QuestionBankRowDto>> GetBankAsync(string creatorId, string? search, QuestionType? type, int take, CancellationToken ct)
        {
            var q = _db.Questions.AsNoTracking()
                .Where(q => q.CreatedByUserId == creatorId);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();
                q = q.Where(q => q.Text.Contains(s));
            }

            if (type.HasValue)
                q = q.Where(q => q.Type == type.Value);

            if (take < 0) take = QuestionBankPageSizeDefault;

            return q
            .OrderByDescending(x => x.Id)
            .Take(take)
            .Select(x => new QuestionBankRowDto
            {
                Id = x.Id,
                Text = x.Text,
                Type = x.Type,
                Points = x.Points,
            })
            .ToListAsync(ct);
        }

        public Task<bool> IsOwnedByTeacherAsync(string questionId, string creatorId, CancellationToken ct)
            => _db.Questions.AsNoTracking()
                .AnyAsync(q => q.Id == questionId && q.CreatedByUserId == creatorId);
    }
}
