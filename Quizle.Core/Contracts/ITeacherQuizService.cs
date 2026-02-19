using Quizle.Core.Dtos;
using Quizle.Core.Types;

namespace Quizle.Core.Contracts
{
    public interface ITeacherQuizService
    {
        Task<List<QuizListItemDto>> GetQuizzesForGroupAsync(string teacherId, string groupId, CancellationToken ct);

        Task<string> CreateQuizAsync(string teacherId, QuizCreateDto vm, CancellationToken ct);

        Task<QuizBuilderDto> GetBuilderAsync(string teacherId, string quizId, string? search, QuestionType? type, CancellationToken ct);

        Task AddQuestionAsync(string teacherId, string quizId, string questionId, CancellationToken ct);

        Task RemoveQuestionAsync(string teacherId, string quizId, string questionId, CancellationToken ct);

        Task MoveAsync(string teacherId, string quizId, string questionId, int direction, CancellationToken ct);
    }
}
