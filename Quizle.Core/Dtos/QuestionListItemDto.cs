using Quizle.Core.Types;

namespace Quizle.Core.Dtos
{
    public class QuestionListItemDto
    {
        public string Id { get; set; } = default!;
        public string Text { get; set; } = default!;
        public QuestionType Type { get; set; }
        public int Points { get; set; }
        public int OptionsCount { get; set; }
    }
}
