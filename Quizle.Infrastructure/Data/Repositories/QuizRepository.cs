using Microsoft.EntityFrameworkCore;
using Quizle.Core.Contracts;
using Quizle.Core.Dtos;
using Quizle.Core.Entities;
using Quizle.Core.Types;

namespace Quizle.Infrastructure.Data.Repositories
{
    public class QuizRepository : IQuizRepository
    {
        private readonly ApplicationDbContext _db;

        public QuizRepository(ApplicationDbContext db) => _db = db;

        public Task AddQuizAsync(Quiz quiz, CancellationToken ct)
            => _db.Quizzes.AddAsync(quiz, ct).AsTask();

        public async Task AddQuizQuestionAsync(string quizId, string questionId, int order, CancellationToken ct)
        {
            var link = new QuizQuestion
            {
                QuizId = quizId,
                QuestionId = questionId,
                Order = order
            };

            await _db.QuizQuestions.AddAsync(link, ct);
        }

        public async Task CompactOrdersAsync(string quizId, CancellationToken ct)
        {
            var list = await _db.QuizQuestions
                .Where(x => x.QuizId == quizId)
                .OrderBy(x => x.Order)
                .ToListAsync(ct);

            var order = 1;
            foreach (var qq in list)
                qq.Order = order++;

            await _db.SaveChangesAsync(ct);
        }

        public Task<List<QuizDto>> GetActiveGroupQuizzesForStudentAsync(string studentId, CancellationToken ct)
            => _db.Quizzes
            .AsNoTracking()
            .Where(q =>
                q.Status == QuizStatus.Active &&
                q.ActiveUntilUtc != null &&
                q.ActiveUntilUtc > DateTime.UtcNow &&
                q.SchoolGroup.Students.Any(s => s.Id == studentId))
            .Select(q => new QuizDto
            {
                Id = q.Id,
                Title = q.Title,
                GroupName = q.SchoolGroup.Name,
                ActiveUntilUtc = q.ActiveUntilUtc,
                DurationMinutes = q.DurationMinutes
            })
            .ToListAsync(ct);

        public Task<Quiz?> GetByIdAsync(string id, CancellationToken ct)
            => _db.Quizzes.FirstOrDefaultAsync(x => x.Id == id, ct);

        public async Task<int> GetNextOrderAsync(string quizId, CancellationToken ct)
        {
            var max = await _db.QuizQuestions
                .AsNoTracking()
                .Where(x => x.QuizId == quizId)
                .MaxAsync(x => (int?)x.Order, ct);

            return (max ?? 0) + 1;
        }

        public Task<QuizHeaderDto?> GetQuizHeaderAsync(string teacherId, string quizId, CancellationToken ct)
            => _db.Quizzes
            .AsNoTracking()
            .Where(q => q.Id == quizId && q.SchoolGroup.TeacherId == teacherId)
            .Select(q => new QuizHeaderDto(
                q.Id,
                q.SchoolGroupId,
                q.Title,
                q.DurationMinutes,
                q.Status
            ))
            .FirstOrDefaultAsync(ct);

        public Task<List<QuizListRowDto>> GetQuizzesForGroupAsync(string teacherId, string groupId, CancellationToken ct)
            => _db.Quizzes
            .AsNoTracking()
            .Where(q => q.SchoolGroupId == groupId && q.SchoolGroup.TeacherId == teacherId)
            .OrderByDescending(q => q.Id)
            .Select(q => new QuizListRowDto(
                q.Id,
                q.Title,
                q.DurationMinutes,
                q.Status,
                q.QuizQuestions.Count
            ))
            .ToListAsync(ct);

        public Task<Quiz?> GetReadOnlyByIdAsync(string id, CancellationToken ct)
            => _db.Quizzes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);

        public Task<List<SelectedQuizQuestionDto>> GetSelectedQuestionsAsync(string quizId, CancellationToken ct)
            => _db.QuizQuestions
            .AsNoTracking()
            .Where(x => x.QuizId == quizId)
            .OrderBy(x => x.Order)
            .Select(x => new SelectedQuizQuestionDto(
                x.QuestionId,
                x.Order,
                x.Question.Text,
                x.Question.Type,
                x.Question.Points
            ))
            .ToListAsync(ct);

        public Task<bool> QuizHasQuestionAsync(string quizId, string questionId, CancellationToken ct)
            => _db.QuizQuestions
            .AsNoTracking()
            .AnyAsync(x => x.QuizId == quizId && x.QuestionId == questionId, ct);

        public async Task RemoveQuizQuestionAsync(string quizId, string questionId, CancellationToken ct)
        {
            var link = await _db.QuizQuestions
                .FirstOrDefaultAsync(x => x.QuizId == quizId && x.QuestionId == questionId, ct);

            if (link == null) return;

            _db.QuizQuestions.Remove(link);
        }

        public Task SaveChangesAsync(CancellationToken ct)
            => _db.SaveChangesAsync(ct);

        public async Task SwapOrderAsync(string quizId, int orderA, int orderB, CancellationToken ct)
        {
            if (orderA == orderB) return;

            var items = await _db.QuizQuestions
                .Where(x => x.QuizId == quizId && (x.Order == orderA || x.Order == orderB))
                .ToListAsync(ct);

            if (items.Count != 2) return;

            var a = items.First(x => x.Order == orderA);
            var b = items.First(x => x.Order == orderB);

            // Avoid UNIQUE(QuizId, Order) conflict by using a temporary order.
            // Any value not used in the quiz is fine; 0 is usually safe if your orders start from 1.
            a.Order = 0;
            await _db.SaveChangesAsync(ct);

            b.Order = orderA;
            await _db.SaveChangesAsync(ct);

            a.Order = orderB;
            await _db.SaveChangesAsync(ct);
        }

        public Task<bool> TeacherOwnsGroupAsync(string teacherId, string groupId, CancellationToken ct)
            => _db.SchoolGroups.AsNoTracking()
            .AnyAsync(g => g.Id == groupId && g.TeacherId == teacherId, ct);

        public Task<bool> TeacherOwnsQuizAsync(string teacherId, string quizId, CancellationToken ct)
             => _db.Quizzes.AsNoTracking()
            .AnyAsync(q => q.Id == quizId && q.SchoolGroup.TeacherId == teacherId, ct);
    }
}
