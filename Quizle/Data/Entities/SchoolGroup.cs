using System.ComponentModel.DataAnnotations;

namespace Quizle.Data.Entities
{
    public class SchoolGroup
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required, MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Description { get; set; }

        // Navigation properties

        public ICollection<ApplicationUser> Students { get; set; } = [];

        public ICollection<Quiz> Quizzes { get; set; } = [];
    }
}
