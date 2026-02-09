using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quizle.Infrastructure.Data.Entities
{
    public class StudentAnswer
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [ForeignKey(nameof(QuizAttempt))]
        public string QuizAttemptId { get; set; } = default!;

        [ForeignKey(nameof(Question))]
        public string QuestionId { get; set; } = default!;

        // За MultipleChoice:
        public string? SelectedOptionId { get; set; }

        // За Numeric:
        public decimal? NumericValue { get; set; }

        // За ShortText:
        [StringLength(2000)]
        public string? TextValue { get; set; }

        public int? AwardedPoints { get; set; } // попълва се при оценяване

        // Navigation properties

        public QuizAttempt QuizAttempt { get; set; } = null!;
        public Question Question { get; set; } = null!;
    }
}
