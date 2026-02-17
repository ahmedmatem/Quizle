using Quizle.Core.Types;
using System.ComponentModel.DataAnnotations;

namespace Quizle.Web.Areas.Teacher.Models
{
    public class QuestionEditVm
    {
        public string? Id { get; set; } // null for create

        [Required]
        public string Text { get; set; } = string.Empty;

        [Range(1, 100)]
        public int Points { get; set; } = 1;

        [Required]
        public QuestionType Type { get; set; }

        // Numeric
        public decimal? CorrectNumeric { get; set; }
        public decimal? NumericTolerance { get; set; }

        // MultipleChoice
        public List<OptionInputVm> Options { get; set; } = new();
        public int? CorrectIndex { get; set; } // index in Options list (0-based)
    }

    public class OptionInputVm
    {
        public string? Id { get; set; } // existing option id (for edit)
        [Required, MaxLength(250)]
        public string Text { get; set; } = string.Empty;
    }
}
