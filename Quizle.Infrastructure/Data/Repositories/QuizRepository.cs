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

        public Task<Quiz?> GetReadOnlyByIdAsync(string id, CancellationToken ct)
            => _db.Quizzes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
    }
}
