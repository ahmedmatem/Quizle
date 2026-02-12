using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quizle.Infrastructure.Data.Entities
{
    public class SchoolGroup
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string TeacherId { get; set; } = null!;

        [Required, MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Description { get; set; }

        // Navigation properties

        public ICollection<ApplicationUser> Students { get; set; } = [];

        public ICollection<Quiz> Quizzes { get; set; } = [];
    }
}
