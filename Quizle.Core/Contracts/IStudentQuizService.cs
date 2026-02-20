using Quizle.Core.Dtos;

namespace Quizle.Core.Contracts
{
    public interface IStudentQuizService
    {
        Task<StudentDashboardDto> GetDashboardAsync(string studentId, CancellationToken ct);

        /// returns attemptId (for redirect to Solve)
        Task<string> StartOrResumeAsync(string studentId, string quizId, CancellationToken ct);
    }
}
