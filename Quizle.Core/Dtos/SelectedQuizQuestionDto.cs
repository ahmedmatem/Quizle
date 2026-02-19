using Quizle.Core.Types;

namespace Quizle.Core.Dtos
{
    public sealed record SelectedQuizQuestionDto(
    string QuestionId,
    int Order,
    string Text,
    QuestionType Type,
    int Points);
}
