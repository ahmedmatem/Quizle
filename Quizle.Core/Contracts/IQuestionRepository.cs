using Quizle.Core.Entities;

namespace Quizle.Core.Contracts
{
    public interface IQuestionRepository
    {
        Task<Question> GetWithOptionsAsync(string questionId, CancellationToken ct);
        Task<List<string>> GetOrderedQuestionsAsync(string quizId, CancellationToken ct);
        Task<bool> BelongsToQuizAsync(string quizId, string questionId, CancellationToken ct);
        Task<Dictionary<string, Question>> GetAllAsync(List<string> questionIds, CancellationToken ct);
        Task<Question?> GetAsync(string questionId, string creatorId, CancellationToken ct);
        Task<Question?> GetIncludeOptionsAsync(string qId, string creatorId, CancellationToken ct);
        IQueryable<Question> GetByCreator(string creatorId);
        Task AddAsync(Question question, CancellationToken ct);
        Task AddOptionsAsync(IEnumerable<ChoiceOption> options, CancellationToken ct);
        void Remove(Question question);
        void RemoveChoiceOption(ChoiceOption option);
        void AddChoiceOption(ChoiceOption option);
        Task SaveChangesAsync(CancellationToken ct);
    }
}
