using Quizle.Core.Dtos;
using Quizle.Core.Entities;
using Quizle.Core.Types;

namespace Quizle.Core.Contracts
{
    public interface IQuizRepository
    {
        public Task<Quiz?> GetByIdAsync(string id, CancellationToken ct);
        public Task<Quiz?> GetReadOnlyByIdAsync(string id, CancellationToken ct);
        public Task<List<QuizDto>> GetActiveGroupQuizzesForStudentAsync(string studentId, CancellationToken ct);
        // ---- Security checks ----
        Task<bool> TeacherOwnsGroupAsync(string teacherId, string groupId, CancellationToken ct);
        Task<bool> TeacherOwnsQuizAsync(string teacherId, string quizId, CancellationToken ct);

        // ---- List + Create ----
        Task<List<QuizListRowDto>> GetQuizzesForGroupAsync(string teacherId, string groupId, CancellationToken ct);
        Task AddQuizAsync(Quiz quiz, CancellationToken ct);
        Task SaveChangesAsync(CancellationToken ct);

        // ---- Builder load (header + selected questions ordered) ----
        Task<QuizHeaderDto?> GetQuizHeaderAsync(string teacherId, string quizId, CancellationToken ct);
        Task<List<SelectedQuizQuestionDto>> GetSelectedQuestionsAsync(string quizId, CancellationToken ct);

        // ---- Join table operations ----
        Task<bool> QuizHasQuestionAsync(string quizId, string questionId, CancellationToken ct);
        Task<int> GetNextOrderAsync(string quizId, CancellationToken ct);

        Task AddQuizQuestionAsync(string quizId, string questionId, int order, CancellationToken ct);
        Task RemoveQuizQuestionAsync(string quizId, string questionId, CancellationToken ct);

        /// <summary>Swaps orders between two items (used for Up/Down).</summary>
        Task SwapOrderAsync(string quizId, int orderA, int orderB, CancellationToken ct);

        /// <summary>Compacts orders to 1..n after removal.</summary>
        Task CompactOrdersAsync(string quizId, CancellationToken ct);

        Task<QuizStatus?> GetStatusAsync(string quizId, CancellationToken ct);

        Task ActivateAsync(string quizId, DateTime fromUtc, DateTime untilUtc, CancellationToken ct);

        Task CloseAsync(string quizId, DateTime closedAtUtc, CancellationToken ct);
    }
}
