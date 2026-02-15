using Quizle.Core.Contracts;
using Quizle.Core.Dtos;

namespace Quizle.Core.Services
{
    public class StudentDashboardService : IStudentDashboardService
    {
        private readonly IQuizRepository _quizRepository;
        private readonly IQuizAttemptRepository _attemptRepository;

        public StudentDashboardService(
            IQuizRepository quizRepository,
            IQuizAttemptRepository attemptRepository)
        {
            _quizRepository = quizRepository;
            _attemptRepository = attemptRepository;
        }

        public async Task<List<QuizDto>> GetStudentDashboardActiveQuizzesAsync(string studentId, CancellationToken ct)
        {
            var quizzes = await _quizRepository.GetActiveGroupQuizzesForStudentAsync(studentId, ct);

            // Load existing attempts for these quizzes (1 attempt rule)
            var quizIds = quizzes.Select(x => x.Id).ToList();

            var attempts = await _attemptRepository.GetInRangeAsync(quizIds, studentId, ct);

            return [.. quizzes.Select(q =>
            {
                var a = attempts.FirstOrDefault(x => x.QuizId == q.Id);

                return new QuizDto
                {
                    Id = q.Id,
                    Title = q.Title,
                    GroupName = q.GroupName,
                    ActiveUntilUtc = q.ActiveUntilUtc,
                    DurationMinutes = q.DurationMinutes,

                    HasAttempt = a != null,
                    IsSubmitted = a?.SubmittedAtUtc != null,
                    StartedAtUtc = a?.StartedAtUtc,
                    Score = a?.Score,
                    MaxScore = a?.MaxScore
                };
            })];
        } 
    }
}
