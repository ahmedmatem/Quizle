using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quizle.Core.Entities
{
    public class QuizAttempt
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [ForeignKey(nameof(Quiz))]
        public string QuizId { get; set; } = default!;

        [ForeignKey(nameof(Student))]
        public string StudentId { get; set; } = null!;

        public DateTime StartedAtUtc { get; set; }
        public DateTime? SubmittedAtUtc { get; set; }

        public int? Score { get; set; } // общ резултат
        public int? MaxScore { get; set; }

        // Navigation properties

        public Quiz Quiz { get; set; } = null!;

        public ApplicationUser Student { get; set; } = null!;

        public ICollection<StudentAnswer> Answers { get; set; } = [];
    }
}
