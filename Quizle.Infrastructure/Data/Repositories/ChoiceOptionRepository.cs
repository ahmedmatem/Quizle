using Microsoft.EntityFrameworkCore;
using Quizle.Core.Contracts;

namespace Quizle.Infrastructure.Data.Repositories
{
    public class ChoiceOptionRepository : IChoiceOptionRepository
    {
        private readonly ApplicationDbContext _db;

        public ChoiceOptionRepository(ApplicationDbContext db) => _db = db;

        public Task<bool> BelongsToQuestion(string questionId, string choiceOptId, CancellationToken ct)
            => _db.ChoiceOptions.AnyAsync(co => co.QuestionId == questionId && co.Id == choiceOptId, ct);
    }
}
