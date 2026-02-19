using Quizle.Core.Types;

namespace Quizle.Web.Areas.Teacher.Models
{
    public class QuizBuilderVm
    {
        public string QuizId { get; set; } = default!;
        public string SchoolGroupId { get; set; } = default!;
        public string QuizTitle { get; set; } = default!;
        public int DurationMinutes { get; set; }
        public QuizStatus Status { get; set; }

        public List<QuizQuestionRowVm> Selected { get; set; } = new();

        // Add-from-bank section
        public string? Search { get; set; }
        public QuestionType? Type { get; set; }
        public List<QuestionBankRowVm> Bank { get; set; } = new();

        public int TotalPoints => Selected.Sum(x => x.Points);
    }

    public class QuizQuestionRowVm
    {
        public string QuestionId { get; set; } = default!;
        public int Order { get; set; }
        public string Text { get; set; } = default!;
        public QuestionType Type { get; set; }
        public int Points { get; set; }
    }

    public class QuestionBankRowVm
    {
        public string Id { get; set; } = default!;
        public string Text { get; set; } = default!;
        public QuestionType Type { get; set; }
        public int Points { get; set; }
        public bool AlreadyAdded { get; set; }
    }
}
