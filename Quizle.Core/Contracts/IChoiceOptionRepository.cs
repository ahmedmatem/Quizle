namespace Quizle.Core.Contracts
{
    public interface IChoiceOptionRepository
    {
        Task<bool> BelongsToQuestion(string questionId, string choiceOptId, CancellationToken ct);
    }
}
