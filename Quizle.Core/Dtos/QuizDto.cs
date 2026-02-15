namespace Quizle.Core.Dtos
{
    public class QuizDto
    {
        public string Id { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string GroupName { get; set; } = default!;

        public DateTime? ActiveUntilUtc { get; set; }
        public int DurationMinutes { get; set; }

        public bool HasAttempt { get; set; }
        public bool IsSubmitted { get; set; }
        public DateTime? StartedAtUtc { get; set; }
        public int? Score { get; set; }
        public int? MaxScore { get; set; }
    }
}
