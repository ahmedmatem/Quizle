using Quizle.Core.Entities;

namespace Quizle.Core.Contracts
{
    public interface IQuizAttemptRepository
    {
        Task<QuizAttempt?> GetAsync(string quizId, string studentId, CancellationToken ct);
        Task<int> CreateAsync(QuizAttempt attempt, CancellationToken ct);
        Task<QuizAttempt?> GetWithQuizAsync(string attemptId, CancellationToken ct);
        Task<int> Submit(QuizAttempt attempt, CancellationToken ct);
    }
}
