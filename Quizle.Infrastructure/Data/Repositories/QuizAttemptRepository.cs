using Microsoft.EntityFrameworkCore;
using Quizle.Core.Contracts;
using Quizle.Core.Entities;

namespace Quizle.Infrastructure.Data.Repositories
{
    public class QuizAttemptRepository : IQuizAttemptRepository
    {
        private readonly ApplicationDbContext _db;
        public QuizAttemptRepository(ApplicationDbContext db) => _db = db;

        public Task<int> CreateAsync(QuizAttempt attempt)
        {
            _db.QuizAttempts.Add(attempt);
            return _db.SaveChangesAsync();
        }

        public Task<QuizAttempt?> GetAsync(string quizId, string studentId, CancellationToken ct)
            => _db.QuizAttempts.FirstOrDefaultAsync(x => x.QuizId == quizId && x.StudentId == studentId, ct);
    }
}
