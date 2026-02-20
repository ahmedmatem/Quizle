namespace Quizle.Core.Dtos
{
    public class StudentDashboardDto
    {
        public List<ActiveQuizCardDto> ActiveQuizzes { get; set; } = new();
        public string? Error { get; set; }
    }

    public class ActiveQuizCardDto
    {
        public string QuizId { get; set; } = default!;
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
