using Quizle.Core.Types;

namespace Quizle.Core.Dtos
{
    public class SolveQuestionDto
    {
        public string AttemptId { get; set; } = default!;
        public string QuizId { get; set; } = default!;
        public string QuizTitle { get; set; } = default!;

        public int Index { get; set; }      // 0-based
        public int Total { get; set; }

        public DateTime StartedAtUtc { get; set; }
        public DateTime ActiveUntilUtc { get; set; }

        public string QuestionId { get; set; } = default!;
        public string Text { get; set; } = default!;
        public QuestionType Type { get; set; }
        public int Points { get; set; }

        public List<OptionDto> Options { get; set; } = new();

        // existing answer (for Back)
        public string? SelectedOptionId { get; set; }
        public decimal? NumericValue { get; set; }
        public string? TextValue { get; set; }
    }
}
