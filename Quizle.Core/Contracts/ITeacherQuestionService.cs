using Quizle.Core.Dtos;
using Quizle.Core.Types;

namespace Quizle.Core.Contracts
{
    public interface ITeacherQuestionService
    {
        Task<QuestionIndexDto> GetIndexAsync(string teacherId, string? search, QuestionType? type, CancellationToken ct);
        Task<QuestionEditDto> GetForEditAsync(string teacherId, string id, CancellationToken ct);
        Task<string> CreateAsync(string teacherId, QuestionEditDto dto, CancellationToken ct);
        Task UpdateAsync(string teacherId, QuestionEditDto vm, CancellationToken ct);
        Task SoftDeleteAsync(string teacherId, string id, CancellationToken ct);
    }
}
