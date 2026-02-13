namespace Quizle.Core.Entities
{
    public class QuizQuestion
    {
        public string QuizId { get; set; } = default!;
        public Quiz Quiz { get; set; } = default!;

        public string QuestionId { get; set; } = default!;
        public Question Question { get; set; } = default!;

        public int Order { get; set; }
    }
}
