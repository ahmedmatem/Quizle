using Quizle.Data.Types;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quizle.Data.Entities
{
    public class Quiz
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [ForeignKey(nameof(SchoolGroup))]
        public string SchoolGroupId { get; set; } = default!;


        [Required, MaxLength(120)]
        public string Title { get; set; } = null!;

        [Range(1, 180)]
        public int DurationMinutes { get; set; }

        public QuizStatus Status { get; set; } = QuizStatus.Draft;

        public DateTime? ActiveFromUtc { get; set; } // когато учителят го активира

        // Navigation properties

        public SchoolGroup SchoolGroup { get; set; } = null!;
        public ICollection<Question> Questions { get; set; } = [];
        public ICollection<QuizAttempt> QuizAttempts { get; set; } = [];
    }
}
