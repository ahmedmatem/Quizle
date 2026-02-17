using Microsoft.EntityFrameworkCore;
using Quizle.Core.Contracts;
using Quizle.Core.Dtos;
using Quizle.Core.Entities;

namespace Quizle.Infrastructure.Data.Repositories
{
    public class SchoolGroupRepository : ISchoolGroupRepository
    {
        private readonly ApplicationDbContext _db;

        public SchoolGroupRepository(ApplicationDbContext db) => _db = db;

        public Task<int> AddStudentInGroupAsync(SchoolGroup group, ApplicationUser student, CancellationToken ct)
        {
            group.Students.Add(student);
            return _db.SaveChangesAsync(ct);
        }

        public async Task<string> CreateAsync(string teacherId, CreateGroupDto groupDto, CancellationToken ct)
        {
            var group = new SchoolGroup()
            {
                Name = groupDto.Name,
                Description = string.IsNullOrWhiteSpace(groupDto.Description) ? null : groupDto.Description.Trim(),
                TeacherId = teacherId,
            };

            _db.SchoolGroups.Add(group);
            await _db.SaveChangesAsync(ct);

            return group.Id;
        }

        public Task<SchoolGroup?> GetByIdAsync(string id, CancellationToken ct)
            => _db.SchoolGroups.FirstOrDefaultAsync(x => x.Id == id, ct);

        public Task<SchoolGroup?> GetReadOnlyByIdAsync(string id, CancellationToken ct)
            => _db.SchoolGroups.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);

        public IQueryable<SchoolGroup> GetTeacherGroup(string teacherId, string groupId)
            => _db.SchoolGroups.AsNoTracking()
            .Where(g => g.TeacherId == teacherId && g.Id == groupId);

        public Task<SchoolGroup?> GetTeacherGroupIncludeStudentsAsync(string teacherId, string groupId, CancellationToken ct)
            => _db.SchoolGroups.Include(g => g.Students)
            .FirstOrDefaultAsync(g => g.TeacherId == teacherId && g.Id == groupId, ct);

        public IQueryable<SchoolGroup> GetTeacherGroups(string teacherId)
            => _db.SchoolGroups.AsNoTracking().Where(g => g.TeacherId == teacherId)
            .OrderBy(g => g.Name);

        public Task<bool> IsStudentInGroupAsync(string groupId, string studentId, CancellationToken ct)
            => _db.SchoolGroups.Where(x => x.Id == groupId)
            .AnyAsync(x => x.Students.Any(s => s.Id == studentId), ct);

        public Task<int> RemoveStudentFromGroupAsync(SchoolGroup group, ApplicationUser student, CancellationToken ct)
        {
            group.Students.Remove(student);
            return _db.SaveChangesAsync(ct);
        }
    }
}
