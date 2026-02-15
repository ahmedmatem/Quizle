using Quizle.Core.Dtos;
using Quizle.Core.Entities;

namespace Quizle.Core.Contracts
{
    public interface IQuizRepository
    {
        public Task<Quiz?> GetByIdAsync(string id, CancellationToken ct);
        public Task<Quiz?> GetReadOnlyByIdAsync(string id, CancellationToken ct);
        public Task<List<QuizDto>> GetActiveGroupQuizzesForStudentAsync(string studentId, CancellationToken ct);
    }
}
