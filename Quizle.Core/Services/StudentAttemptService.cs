using Quizle.Core.Contracts;
using Quizle.Core.Dtos;
using Quizle.Core.Entities;
using Quizle.Core.Types;

namespace Quizle.Core.Services
{
    public class StudentAttemptService : IStudentAttemptService
    {
        public async Task<SolveQuestionDto> GetSolveVmAsync(string attemptId, int index, CancellationToken ct)
        {
            //var quiz = await _db.Quizzes
            //.AsNoTracking()
            //.FirstOrDefaultAsync(q => q.Id == quizId, ct);

            //if (quiz == null) throw new InvalidOperationException("Quiz not found.");

            //if (quiz.Status != QuizStatus.Active)
            //    throw new InvalidOperationException("Quiz is not active.");

            //if (!quiz.ActiveUntilUtc.HasValue)
            //    throw new InvalidOperationException("Quiz active window is not configured.");

            //if (DateTime.UtcNow > quiz.ActiveUntilUtc.Value)
            //    throw new InvalidOperationException("Quiz has ended.");

            //// optional: validate student is in group (skip if you do it elsewhere)
            //var inGroup = await _db.SchoolGroups
            //    .Where(g => g.Id == quiz.SchoolGroupId)
            //    .AnyAsync(g => g.Students.Any(u => u.Id == studentId), ct);

            //if (!inGroup) throw new InvalidOperationException("You are not in this group.");

            //// enforce 1 attempt: get existing or create
            //var attempt = await _db.QuizAttempts
            //    .FirstOrDefaultAsync(a => a.QuizId == quizId && a.StudentId == studentId, ct);

            //if (attempt != null) return attempt;

            //attempt = new QuizAttempt
            //{
            //    QuizId = quizId,
            //    StudentId = studentId,
            //    StartedAtUtc = DateTime.UtcNow
            //};

            //_db.QuizAttempts.Add(attempt);
            //await _db.SaveChangesAsync(ct);

            //return attempt;
            throw new NotImplementedException();
        }

        public Task<QuizAttempt> StartOrGetAttemptAsync(string quizId, string studentId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
