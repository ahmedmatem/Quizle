using System.ComponentModel.DataAnnotations;

namespace Quizle.Core.Dtos
{
    public class QuizCreateDto
    {
        public string SchoolGroupId { get; set; } = default!;
        public string Title { get; set; } = "";
        public int DurationMinutes { get; set; } = 10;
    }
}
