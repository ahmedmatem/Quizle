using Quizle.Core.Contracts;
using Quizle.Core.Dtos;
using Quizle.Core.Entities;
using Quizle.Core.Types;

namespace Quizle.Core.Services
{
    public class TeacherQuizService : ITeacherQuizService
    {
        private readonly IQuizRepository _quizzes;
        private readonly IQuestionRepository _questions;

        public TeacherQuizService(IQuizRepository quizzes, IQuestionRepository questions)
        {
            _quizzes = quizzes;
            _questions = questions;
        }

        public async Task AddQuestionAsync(string teacherId, string quizId, string questionId, CancellationToken ct)
        {
            // security (and load status)
            var header = await _quizzes.GetQuizHeaderAsync(teacherId, quizId, ct);
            if (header == null) throw new InvalidOperationException("Quiz not found or access denied.");
            
            EnsureDraft(header.Status);

            // question must be teacher-owned
            if (!await _questions.IsOwnedByTeacherAsync(questionId, teacherId, ct))
                throw new InvalidOperationException("Question not found or not yours.");

            // prevent duplicates
            if (await _quizzes.QuizHasQuestionAsync(quizId, questionId, ct))
                return;

            var nextOrder = await _quizzes.GetNextOrderAsync(quizId, ct);

            await _quizzes.AddQuizQuestionAsync(quizId, questionId, nextOrder, ct);
            await _quizzes.SaveChangesAsync(ct);
        }

        public async Task<string> CreateQuizAsync(string teacherId, QuizCreateDto dto, CancellationToken ct)
        {
            if (!await _quizzes.TeacherOwnsGroupAsync(teacherId, dto.SchoolGroupId, ct))
                throw new InvalidOperationException("Access denied.");

            var quiz = new Quiz
            {
                Id = Guid.NewGuid().ToString(),
                SchoolGroupId = dto.SchoolGroupId,
                Title = dto.Title.Trim(),
                DurationMinutes = dto.DurationMinutes,
                Status = QuizStatus.Draft
            };

            await _quizzes.AddQuizAsync(quiz, ct);
            await _quizzes.SaveChangesAsync(ct);
            return quiz.Id;
        }

        public async Task<QuizBuilderDto> GetBuilderAsync(string teacherId, string quizId, string? search, QuestionType? type, CancellationToken ct)
        {
            var header = await _quizzes.GetQuizHeaderAsync(teacherId, quizId, ct);
            if (header == null)
                throw new InvalidOperationException("Quiz not found or access denied.");

            var selected = await _quizzes.GetSelectedQuestionsAsync(quizId, ct);

            var bank = await _questions.GetBankAsync(
                teacherId,
                search,
                type,
                take: 50,
                ct);

            var selectedIds = selected.Select(x => x.QuestionId).ToHashSet();

            return new QuizBuilderDto
            {
                QuizId = header.QuizId,
                SchoolGroupId = header.SchoolGroupId,
                QuizTitle = header.Title,
                DurationMinutes = header.DurationMinutes,
                Status = header.Status,

                Selected = selected.Select(x => new QuizQuestionRowDto
                {
                    QuestionId = x.QuestionId,
                    Order = x.Order,
                    Text = x.Text,
                    Type = x.Type,
                    Points = x.Points
                }).ToList(),

                Search = search,
                Type = type,

                Bank = [.. bank.Select(x => new QuestionBankRowDto
                {
                    Id = x.Id,
                    Text = x.Text,
                    Type = x.Type,
                    Points = x.Points,
                    AlreadyAdded = selectedIds.Contains(x.Id)
                })]
            };
        }

        public async Task<List<QuizListItemDto>> GetQuizzesForGroupAsync(string teacherId, string groupId, CancellationToken ct)
        {
            // security
            if (!await _quizzes.TeacherOwnsGroupAsync(teacherId, groupId, ct))
                throw new InvalidOperationException("Access denied.");

            var rows = await _quizzes.GetQuizzesForGroupAsync(teacherId, groupId, ct);

            return rows.Select(r => new QuizListItemDto
            {
                Id = r.Id,
                Title = r.Title,
                DurationMinutes = r.DurationMinutes,
                Status = r.Status,
                QuestionsCount = r.QuestionsCount
            }).ToList();
        }

        public async Task MoveAsync(string teacherId, string quizId, string questionId, int direction, CancellationToken ct)
        {
            var header = await _quizzes.GetQuizHeaderAsync(teacherId, quizId, ct);
            if (header == null) throw new InvalidOperationException("Quiz not found or access denied.");
            
            EnsureDraft(header.Status);

            // We need current order + bounds. Use selected list.
            var selected = await _quizzes.GetSelectedQuestionsAsync(quizId, ct);
            var item = selected.FirstOrDefault(x => x.QuestionId == questionId);
            if (item == null) return;

            var targetOrder = item.Order + direction;
            if (targetOrder < 1 || targetOrder > selected.Count) return;

            await _quizzes.SwapOrderAsync(quizId, item.Order, targetOrder, ct);
        }

        public async Task RemoveQuestionAsync(string teacherId, string quizId, string questionId, CancellationToken ct)
        {
            var header = await _quizzes.GetQuizHeaderAsync(teacherId, quizId, ct);
            if (header == null) throw new InvalidOperationException("Quiz not found or access denied.");
            
            EnsureDraft(header.Status);

            await _quizzes.RemoveQuizQuestionAsync(quizId, questionId, ct);
            await _quizzes.SaveChangesAsync(ct);

            // keep nice order 1..n (important for wizard)
            await _quizzes.CompactOrdersAsync(quizId, ct);
        }

        private static void EnsureDraft(QuizStatus status)
        {
            if (status != QuizStatus.Draft)
                throw new InvalidOperationException("Only Draft quizzes can be edited.");
        }
    }
}
