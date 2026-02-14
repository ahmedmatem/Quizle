using Quizle.Core.Dtos;
using Quizle.Core.Entities;

namespace Quizle.Core.Contracts
{
    public interface IStudentAttemptService
    {
        public Task<QuizAttempt> StartOrGetAttemptAsync(string quizId, string studentId, CancellationToken ct);
        public Task<SolveQuestionDto> GetSolveAsync(string attemptId, int index, CancellationToken ct);
        public Task SaveAnswerAsync(string studentId, SaveAnswerDto req, CancellationToken ct);
        public Task SubmitAsync(string studentId, string attemptId, CancellationToken ct);
    }
}
