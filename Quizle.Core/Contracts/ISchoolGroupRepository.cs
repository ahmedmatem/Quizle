using Quizle.Core.Dtos;
using Quizle.Core.Entities;

namespace Quizle.Core.Contracts
{
    public interface ISchoolGroupRepository
    {
        Task<SchoolGroup?> GetByIdAsync(string id, CancellationToken ct);
        Task<SchoolGroup?> GetReadOnlyByIdAsync(string id, CancellationToken ct);
        Task<bool> IsStudentInGroupAsync(string groupId, string studentId, CancellationToken ct);
        Task<string> CreateAsync(string teacherId, CreateGroupDto groupDto, CancellationToken ct);
        IQueryable<SchoolGroup> GetTeacherGroups(string teacherId);
        IQueryable<SchoolGroup> GetTeacherGroup(string teacherId, string groupId);
        Task<SchoolGroup?> GetTeacherGroupIncludeStudentsAsync(string teacherId, string groupId, CancellationToken ct);
        Task<int> AddStudentInGroupAsync(SchoolGroup group, ApplicationUser student, CancellationToken ct);
        Task<int> RemoveStudentFromGroupAsync(SchoolGroup group, ApplicationUser student, CancellationToken ct);
    }
}
