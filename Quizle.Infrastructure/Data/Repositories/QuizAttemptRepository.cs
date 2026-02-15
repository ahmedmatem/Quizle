using Microsoft.EntityFrameworkCore;
using Quizle.Core.Contracts;
using Quizle.Core.Entities;

namespace Quizle.Infrastructure.Data.Repositories
{
    public class QuizAttemptRepository : IQuizAttemptRepository
    {
        private readonly ApplicationDbContext _db;
        public QuizAttemptRepository(ApplicationDbContext db) => _db = db;

        public Task<int> CreateAsync(QuizAttempt attempt, CancellationToken ct)
        {
            _db.QuizAttempts.Add(attempt);
            return _db.SaveChangesAsync(ct);
        }

        public Task<QuizAttempt?> GetAsync(string quizId, string studentId, CancellationToken ct)
            => _db.QuizAttempts.FirstOrDefaultAsync(x => x.QuizId == quizId && x.StudentId == studentId, ct);

        public Task<List<QuizAttempt>> GetInRangeAsync(ICollection<string> quizIds, string studentId, CancellationToken ct)
        {
            return _db.QuizAttempts
                .AsNoTracking()
                .Where(a => a.StudentId == studentId && 
                            quizIds.Contains(a.QuizId))
                .ToListAsync(ct);
        }

        public Task<QuizAttempt?> GetWithQuizAsync(string attemptId, CancellationToken ct)
            => _db.QuizAttempts.Include(x => x.Quiz).FirstOrDefaultAsync(x => x.Id == attemptId, ct);

        public Task<int> Submit(QuizAttempt attempt, CancellationToken ct)
            => _db.SaveChangesAsync(ct);
    }
}
