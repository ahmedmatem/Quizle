using Microsoft.EntityFrameworkCore;
using Quizle.Core.Contracts;
using Quizle.Core.Dtos;
using Quizle.Core.Entities;
using Quizle.Core.Types;

namespace Quizle.Infrastructure.Data.Repositories
{
    public class StudentQuizRepository : IStudentQuizRepository
    {
        private readonly ApplicationDbContext _db;
        public StudentQuizRepository(ApplicationDbContext db) => _db = db;

        public async Task<string> CreateAttemptAsync(string quizId, string studentId, DateTime startedUtc, CancellationToken ct)
        {
            var attempt = new QuizAttempt
            {
                Id = Guid.NewGuid().ToString(),
                QuizId = quizId,
                StudentId = studentId,
                StartedAtUtc = startedUtc
            };

            await _db.QuizAttempts.AddAsync(attempt, ct);
            await _db.SaveChangesAsync(ct);
            return attempt.Id;
        }

        public async Task<List<StudentActiveQuizRowDto>> GetActiveQuizzesWithAttemptAsync(
        string studentId,
        DateTime nowUtc,
        CancellationToken ct)
        {
            // Active quizzes in groups where student is member
            var query = _db.Quizzes
                .AsNoTracking()
                .Where(q =>
                    q.Status == QuizStatus.Active &&
                    q.ActiveFromUtc != null && q.ActiveUntilUtc != null &&
                    q.ActiveFromUtc <= nowUtc &&
                    q.ActiveUntilUtc > nowUtc &&
                    q.SchoolGroup.Students.Any(s => s.Id == studentId));

            var rows = await query
                .Select(q => new
                {
                    q.Id,
                    q.Title,
                    GroupName = q.SchoolGroup.Name,
                    q.DurationMinutes,
                    q.ActiveUntilUtc,
                    Attempt = q.QuizAttempts
                        .Where(a => a.StudentId == studentId)
                        .OrderByDescending(a => a.StartedAtUtc)
                        .Select(a => new
                        {
                            a.Id,
                            a.StartedAtUtc,
                            a.SubmittedAtUtc,
                            a.Score,
                            a.MaxScore
                        })
                        .FirstOrDefault()
                })
                .OrderBy(x => x.ActiveUntilUtc)
                .ToListAsync(ct);

            return rows.Select(x => new StudentActiveQuizRowDto(
                x.Id,
                x.Title,
                x.GroupName,
                x.DurationMinutes,
                x.ActiveUntilUtc,
                x.Attempt?.Id,
                x.Attempt?.StartedAtUtc,
                x.Attempt?.SubmittedAtUtc,
                x.Attempt?.Score,
                x.Attempt?.MaxScore
            )).ToList();
        }

        public Task<string?> GetOpenAttemptIdAsync(string quizId, string studentId, CancellationToken ct)
        => _db.QuizAttempts
            .AsNoTracking()
            .Where(a => a.QuizId == quizId && a.StudentId == studentId && a.SubmittedAtUtc == null)
            .OrderByDescending(a => a.StartedAtUtc)
            .Select(a => a.Id)
            .FirstOrDefaultAsync(ct);

        public Task<bool> IsQuizActiveForStudentAsync(string quizId, string studentId, DateTime nowUtc, CancellationToken ct)
        => _db.Quizzes
            .AsNoTracking()
            .AnyAsync(q =>
                q.Id == quizId &&
                q.Status == QuizStatus.Active &&
                q.ActiveFromUtc != null && q.ActiveUntilUtc != null &&
                q.ActiveFromUtc <= nowUtc &&
                q.ActiveUntilUtc > nowUtc &&
                q.SchoolGroup.Students.Any(s => s.Id == studentId),
                ct);
    }
}
