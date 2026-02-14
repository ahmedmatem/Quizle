using Quizle.Core.Dtos;
using Quizle.Core.Entities;

namespace Quizle.Core.Contracts
{
    public interface IStudentAnswerRepository
    {
        Task<List<StudentAnswer>> GetAllAsync(string attemptId, CancellationToken ct);
        Task<StudentAnswer?> ExistingAnswerAsync(string attemptId, string questionId, CancellationToken ct);
        Task Upsert(SaveAnswerDto answer, CancellationToken ct);
    }
}
