using Quizle.Core.Types;

namespace Quizle.Core.Dtos
{
    public sealed record QuizHeaderDto(
    string QuizId,
    string SchoolGroupId,
    string Title,
    int DurationMinutes,
    QuizStatus Status);
}
