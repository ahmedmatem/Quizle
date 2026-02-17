using Quizle.Core.Types;

namespace Quizle.Web.Areas.Teacher.Models
{
    public class QuestionIndexVm
    {
        public string? Search { get; set; }
        public QuestionType? Type { get; set; }

        public List<QuestionListItemVm> Items { get; set; } = new();
    }
}
