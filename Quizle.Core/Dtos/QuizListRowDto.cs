using Quizle.Core.Types;

namespace Quizle.Core.Dtos
{
    public sealed record QuizListRowDto(
    string Id,
    string Title,
    int DurationMinutes,
    QuizStatus Status,
    int QuestionsCount);
}
