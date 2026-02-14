using Microsoft.EntityFrameworkCore;
using Quizle.Core.Contracts;
using Quizle.Core.Entities;

namespace Quizle.Infrastructure.Data.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly ApplicationDbContext _db;

        public QuestionRepository(ApplicationDbContext db) => _db = db;

        public async Task<List<string>> GetOrderedQuestionsAsync(string quizId, CancellationToken ct)
            => await _db.QuizQuestions
            .Where(qq => qq.QuizId == quizId)
            .OrderBy(qq => qq.Order)
            .Select(qq => qq.QuestionId)
            .ToListAsync(ct);

        public async Task<Question> GetWithOptionsAsync(string questionId, CancellationToken ct)
            => await _db.Questions.Include(q => q.Options).FirstAsync(q => q.Id == questionId, ct);

        public async Task<bool> BelongsToQuizAsync(string quizId, string questionId, CancellationToken ct)
            => await _db.QuizQuestions.AnyAsync(qq => qq.QuizId == quizId && qq.QuestionId == questionId, ct);
    }
}
