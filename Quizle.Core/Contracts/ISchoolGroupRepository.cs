using Quizle.Core.Entities;

namespace Quizle.Core.Contracts
{
    public interface ISchoolGroupRepository
    {
        Task<SchoolGroup?> GetByIdAsync(string id, CancellationToken ct);
        Task<SchoolGroup?> GetReadOnlyByIdAsync(string id, CancellationToken ct);
        Task<bool> IsStudentInGroupAsync(string groupId, string studentId, CancellationToken ct);
    }
}
