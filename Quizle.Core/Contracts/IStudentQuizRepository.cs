using Quizle.Core.Dtos;

namespace Quizle.Core.Contracts
{
    public interface IStudentQuizRepository
    {
        Task<List<StudentActiveQuizRowDto>> GetActiveQuizzesWithAttemptAsync(
            string studentId,
            DateTime nowUtc,
            CancellationToken ct);

        Task<bool> IsQuizActiveForStudentAsync(
            string quizId,
            string studentId,
            DateTime nowUtc,
            CancellationToken ct);

        Task<string?> GetOpenAttemptIdAsync(
            string quizId,
            string studentId,
            CancellationToken ct);

        Task<string> CreateAttemptAsync(
            string quizId,
            string studentId,
            DateTime startedUtc,
            CancellationToken ct);
    }
}
