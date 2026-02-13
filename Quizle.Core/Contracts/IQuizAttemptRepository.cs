using Quizle.Core.Entities;

namespace Quizle.Core.Contracts
{
    public interface IQuizAttemptRepository
    {
        Task<QuizAttempt?> GetAsync(string quizId, string studentId, CancellationToken ct);

        Task<int> CreateAsync(QuizAttempt attempt);
    }
}
