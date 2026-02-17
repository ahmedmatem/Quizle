using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quizle.Core.Contracts;
using Quizle.Core.Dtos;
using Quizle.Core.Entities;

namespace Quizle.Core.Services
{
    public class TeacherGroupService : ITeacherGroupService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISchoolGroupRepository _groupRepository;

        public TeacherGroupService(
            UserManager<ApplicationUser> userManager,
            ISchoolGroupRepository groupRepository)
        {
            _userManager = userManager;
            _groupRepository = groupRepository;
        }

        public async Task AddStudentByEmailAsync(string teacherId, string groupId, string email, CancellationToken ct)
        {
            email = email.Trim().ToLowerInvariant();

            var group = await _groupRepository
                .GetTeacherGroupIncludeStudentsAsync(teacherId, groupId, ct);

            if (group is null) throw new InvalidOperationException("Group not found or access denied.");

            var student = await _userManager.FindByEmailAsync(email);
            if (student == null) throw new InvalidOperationException("No user with this email.");

            // ensure student role
            if (!await _userManager.IsInRoleAsync(student, "Student")) throw new InvalidOperationException("User is not a Student.");

            if (group.Students.Any(s => s.Id == student.Id))
                return; // already in group

            // add student in group
            await _groupRepository.AddStudentInGroupAsync(group, student, ct);
        }

        public Task<string> CreateAsync(string teacherId, CreateGroupDto dto, CancellationToken ct)
            => _groupRepository.CreateAsync(teacherId, dto, ct);

        public Task<GroupDetailsDto?> GetDetailsAsync(string teacherId, string groupId, CancellationToken ct)
            => _groupRepository.GetTeacherGroup(teacherId, groupId)
            .Select(g => new GroupDetailsDto
            {
                Id = g.Id,
                Name = g.Name,
                Description = g.Description,
                Students = g.Students
                    .OrderBy(s => s.Email)
                    .Select(s => new StudentRowDto
                    {
                        Id = s.Id,
                        Email = s.Email!,
                        FullName = s.FullName
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync(ct);

        public Task<List<GroupListItemDto>> GetMyGroupsAsync(string teacherId, CancellationToken ct)
            => _groupRepository.GetTeacherGroups(teacherId)
            .Select(g => new GroupListItemDto
            {
                Id = g.Id,
                Name = g.Name,
                Description = g.Description,
                StudentsCount = g.Students.Count,
                QuizzesCount = g.Quizzes.Count
            })
            .ToListAsync(ct);

        public async Task RemoveStudentAsync(string teacherId, string groupId, string studentId, CancellationToken ct)
        {
            var group = await _groupRepository
                .GetTeacherGroupIncludeStudentsAsync(teacherId, groupId, ct);
            
            if (group == null) throw new InvalidOperationException("Group not found or access denied.");

            var student = group.Students
                .FirstOrDefault(s => s.Id == studentId);
            if (student == null) return;

            await _groupRepository
                .RemoveStudentFromGroupAsync(group, student, ct);
        }
    }
}
