using System.ComponentModel.DataAnnotations;

namespace Quizle.Web.Areas.Teacher.Models
{
    public class CreateGroupVm
    {
        [Required, MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Description { get; set; }
    }
}
