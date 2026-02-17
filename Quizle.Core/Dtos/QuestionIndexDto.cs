using Quizle.Core.Types;

namespace Quizle.Core.Dtos
{
    public class QuestionIndexDto
    {
        public string? Search { get; set; }
        public QuestionType? Type { get; set; }

        public List<QuestionListItemDto> Items { get; set; } = new();
    }
}
