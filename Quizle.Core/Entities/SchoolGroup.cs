using Quizle.Core.Abstracts;
using System.ComponentModel.DataAnnotations;

namespace Quizle.Core.Entities
{
    public class SchoolGroup : SoftDeletableEntity
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
        public ApplicationUser Teacher { get; set; } = null!;

        public ICollection<ApplicationUser> Students { get; set; } = [];

        public ICollection<Quiz> Quizzes { get; set; } = [];
    }
}
