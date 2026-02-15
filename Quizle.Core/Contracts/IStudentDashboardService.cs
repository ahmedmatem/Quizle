using Quizle.Core.Dtos;

namespace Quizle.Core.Contracts
{
    public interface IStudentDashboardService
    {
        Task<List<QuizDto>> GetStudentDashboardActiveQuizzesAsync(string studentId, CancellationToken ct);
    }
}
