using Microsoft.EntityFrameworkCore;
using Quizle.Core.Contracts;
using Quizle.Core.Entities;

namespace Quizle.Infrastructure.Data.Repositories
{
    public class QuizRepository : IQuizRepository
    {
        private readonly ApplicationDbContext _db;

        public QuizRepository(ApplicationDbContext db) => _db = db;

        public Task<Quiz?> GetByIdAsync(string id, CancellationToken ct)
            => _db.Quizzes.FirstOrDefaultAsync(x => x.Id == id, ct);

        public Task<Quiz?> GetReadOnlyByIdAsync(string id, CancellationToken ct)
            => _db.Quizzes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
    }
}
