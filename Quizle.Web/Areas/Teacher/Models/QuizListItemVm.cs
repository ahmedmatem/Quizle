using Quizle.Core.Types;

namespace Quizle.Web.Areas.Teacher.Models
{
    public class QuizListItemVm
    {
        public string Id { get; set; } = default!;
        public string Title { get; set; } = default!;
        public int DurationMinutes { get; set; }
        public QuizStatus Status { get; set; }
        public int QuestionsCount { get; set; }
    }
}
