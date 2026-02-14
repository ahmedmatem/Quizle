namespace Quizle.Core.Dtos
{
    public class SaveAnswerDto
    {
        public string AttemptId { get; set; } = default!;
        public string QuestionId { get; set; } = default!;

        public string? SelectedOptionId { get; set; }
        public decimal? NumericValue { get; set; }
        public string? TextValue { get; set; }
    }
}
