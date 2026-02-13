using Quizle.Core.Entities;

namespace Quizle.Core.Contracts
{
    public interface IStudentAnswerRepository
    {
        Task<StudentAnswer?> ExistingAnswerAsync(string attemptId, string questionId, CancellationToken ct);
    }
}
