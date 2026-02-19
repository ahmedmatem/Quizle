using System.ComponentModel.DataAnnotations;

namespace Quizle.Web.Areas.Teacher.Models
{
    public class QuizCreateVm
    {
        [Required]
        public string SchoolGroupId { get; set; } = default!;

        [Required, MaxLength(120)]
        public string Title { get; set; } = "";

        [Range(1, 180)]
        public int DurationMinutes { get; set; } = 10;
    }
}
