using Quizle.Core.Dtos;

namespace Quizle.Core.Contracts
{
    public interface ITeacherGroupService
    {
        public Task<List<GroupListItemDto>> GetMyGroupsAsync(string teacherId, CancellationToken ct);
        public Task<string> CreateAsync(string teacherId, CreateGroupDto dto, CancellationToken ct);
        public Task<GroupDetailsDto?> GetDetailsAsync(string teacherId, string groupId, CancellationToken ct);
        public Task AddStudentByEmailAsync(string teacherId, string groupId, string email, CancellationToken ct);
        public Task RemoveStudentAsync(string teacherId, string groupId, string studentId, CancellationToken ct);
    }
}
