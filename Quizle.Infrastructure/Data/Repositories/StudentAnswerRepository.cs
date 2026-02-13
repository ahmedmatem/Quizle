using Microsoft.EntityFrameworkCore;
using Quizle.Core.Contracts;
using Quizle.Core.Entities;

namespace Quizle.Infrastructure.Data.Repositories
{
    public class StudentAnswerRepository : IStudentAnswerRepository
    {
        private readonly ApplicationDbContext _db;

        public StudentAnswerRepository(ApplicationDbContext db) => _db = db;

        public async Task<StudentAnswer?> ExistingAnswerAsync(string attemptId, string questionId, CancellationToken ct)
            => await _db.StudentAnswers
            .AsNoTracking()
            .FirstOrDefaultAsync(sa => sa.QuizAttemptId == attemptId && sa.QuestionId == questionId, ct);
    }
}
