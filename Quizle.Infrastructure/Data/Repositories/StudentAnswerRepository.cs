using Microsoft.EntityFrameworkCore;
using Quizle.Core.Contracts;
using Quizle.Core.Dtos;
using Quizle.Core.Entities;

namespace Quizle.Infrastructure.Data.Repositories
{
    public class StudentAnswerRepository : IStudentAnswerRepository
    {
        private readonly ApplicationDbContext _db;

        public StudentAnswerRepository(ApplicationDbContext db) => _db = db;

        public Task<StudentAnswer?> ExistingAnswerAsync(string attemptId, string questionId, CancellationToken ct)
            => _db.StudentAnswers
            .AsNoTracking()
            .FirstOrDefaultAsync(sa => sa.QuizAttemptId == attemptId && sa.QuestionId == questionId, ct);

        public Task<List<StudentAnswer>> GetAllAsync(string attemptId, CancellationToken ct)
            => _db.StudentAnswers.Where(sa => sa.QuizAttemptId == attemptId).ToListAsync(ct);

        public async Task Upsert(SaveAnswerDto answerDto, CancellationToken ct)
        {
            var answer = await _db.StudentAnswers.FirstOrDefaultAsync(sa => 
                sa.QuizAttemptId == answerDto.AttemptId && 
                sa.QuestionId == answerDto.QuestionId, ct);

            if(answer is null)
            {
                answer = new StudentAnswer
                {
                    QuizAttemptId = answerDto.AttemptId,
                    QuestionId = answerDto.QuestionId,
                };
                _db.StudentAnswers.Add(answer);
            }

            // Overwrite fields; clear others depending on type is optional
            answer.SelectedOptionId = answerDto.SelectedOptionId;
            answer.NumericValue = answerDto.NumericValue;
            answer.TextValue = answerDto.TextValue;

            await _db.SaveChangesAsync(ct);
        }
    }
}
