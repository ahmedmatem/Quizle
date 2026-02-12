using Quizle.Infrastructure.Data.Abstracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quizle.Infrastructure.Data.Entities
{
    public class ChoiceOption : SoftDeletableEntity
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [ForeignKey(nameof(Question))]
        public string QuestionId { get; set; } = default!;

        [Required, MaxLength(250)]
        public string Text { get; set; } = null!;

        // Navigation properties

        public Question Question { get; set; } = null!;
    }
}
