using Microsoft.EntityFrameworkCore;
using Quizle.Core.Contracts;
using Quizle.Core.Entities;

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
    }
}
