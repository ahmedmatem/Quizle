namespace Quizle.Web.Areas.Student.Models
{
    public class SaveAnswerReq
    {
        public string AttemptId { get; set; } = default!;
        public string QuestionId { get; set; } = default!;

        public string? SelectedOptionId { get; set; }
        public decimal? NumericValue { get; set; }
        public string? TextValue { get; set; }
    }
}
