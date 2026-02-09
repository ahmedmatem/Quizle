using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Quizle.Data.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(120)]
        public string? FullName { get; set; }

        // Navigation properties
        public ICollection<SchoolGroup> GroupStudents { get; set; } = [];
        public ICollection<QuizAttempt> QuizAttempts { get; set; } = [];

    }
}
