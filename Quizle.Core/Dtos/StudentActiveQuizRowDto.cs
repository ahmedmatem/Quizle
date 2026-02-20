namespace Quizle.Core.Dtos
{
    public sealed record StudentActiveQuizRowDto(
        string QuizId,
        string Title,
        string GroupName,
        int DurationMinutes,
        DateTime? ActiveUntilUtc,
        string? AttemptId,
        DateTime? StartedAtUtc,
        DateTime? SubmittedAtUtc,
        int? Score,
        int? MaxScore);
}
