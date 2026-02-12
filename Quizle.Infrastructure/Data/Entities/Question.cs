using Quizle.Infrastructure.Data.Types;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quizle.Infrastructure.Data.Entities
{
    public class Question
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Text { get; set; } = null!;

        public QuestionType Type { get; set; }

        [Range(1, 100)]
        public int Points { get; set; } = 1;

        // За Numeric:
        public decimal? CorrectNumeric { get; set; }
        public decimal? NumericTolerance { get; set; } // напр. 0.01

        // За MultipleChoice:
        public string? CorrectOptionId { get; set; }
        public ChoiceOption? CorrectOption { get; set; }

        // Navigation properties

        public ICollection<QuizQuestion> QuizQuestions { get; set; } = [];

        public ICollection<ChoiceOption> Options { get; set; } = [];
    }
}
