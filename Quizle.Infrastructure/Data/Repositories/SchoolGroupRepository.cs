using Microsoft.EntityFrameworkCore;
using Quizle.Core.Contracts;
using Quizle.Core.Entities;

namespace Quizle.Infrastructure.Data.Repositories
{
    public class SchoolGroupRepository : ISchoolGroupRepository
    {
        private readonly ApplicationDbContext _db;

        public SchoolGroupRepository(ApplicationDbContext db) => _db = db;

        public Task<SchoolGroup?> GetByIdAsync(string id, CancellationToken ct)
            => _db.SchoolGroups.FirstOrDefaultAsync(x => x.Id == id, ct);

        public Task<SchoolGroup?> GetReadOnlyByIdAsync(string id, CancellationToken ct)
            => _db.SchoolGroups.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);

        public Task<bool> IsStudentInGroupAsync(string groupId, string studentId, CancellationToken ct)
            => _db.SchoolGroups.Where(x => x.Id == groupId)
            .AnyAsync(x => x.Students.Any(s => s.Id == studentId), ct);
    }
}
