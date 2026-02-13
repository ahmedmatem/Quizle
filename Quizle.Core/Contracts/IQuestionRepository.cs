using Quizle.Core.Entities;

namespace Quizle.Core.Contracts
{
    public interface IQuestionRepository
    {
        Task<Question> GetWithOptionsAsync(string questionId, CancellationToken ct);
        Task<List<string>> GetOrderedQuestionsAsync(string quizId, CancellationToken ct);
    }
}
