using Quizle.Core.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizle.Core.Dtos
{
    public class QuizBuilderDto
    {
        public string QuizId { get; set; } = default!;
        public string SchoolGroupId { get; set; } = default!;
        public string QuizTitle { get; set; } = default!;
        public int DurationMinutes { get; set; }
        public QuizStatus Status { get; set; }

        public List<QuizQuestionRowDto> Selected { get; set; } = new();

        // Add-from-bank section
        public string? Search { get; set; }
        public QuestionType? Type { get; set; }
        public List<QuestionBankRowDto> Bank { get; set; } = new();

        public int TotalPoints => Selected.Sum(x => x.Points);
    }

    public class QuizQuestionRowDto
    {
        public string QuestionId { get; set; } = default!;
        public int Order { get; set; }
        public string Text { get; set; } = default!;
        public QuestionType Type { get; set; }
        public int Points { get; set; }
    }

    public class QuestionBankRowDto
    {
        public string Id { get; set; } = default!;
        public string Text { get; set; } = default!;
        public QuestionType Type { get; set; }
        public int Points { get; set; }
        public bool AlreadyAdded { get; set; }
    }
}
