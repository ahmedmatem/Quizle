using Quizle.Core.Contracts;
using Quizle.Core.Dtos;

namespace Quizle.Core.Services
{
    public class StudentQuizService : IStudentQuizService
    {
        private readonly IStudentQuizRepository _quizRepository;

        public StudentQuizService(IStudentQuizRepository repository) => _quizRepository = repository;

        public async Task<StudentDashboardDto> GetDashboardAsync(string studentId, CancellationToken ct)
        {
            var nowUtc = DateTime.UtcNow;
            var rows = await _quizRepository.GetActiveQuizzesWithAttemptAsync(studentId, nowUtc, ct);

            return new StudentDashboardDto
            {
                ActiveQuizzes = rows.Select(r => new ActiveQuizCardDto
                {
                    QuizId = r.QuizId,
                    Title = r.Title,
                    GroupName = r.GroupName,
                    ActiveUntilUtc = r.ActiveUntilUtc,
                    DurationMinutes = r.DurationMinutes,

                    HasAttempt = !string.IsNullOrWhiteSpace(r.AttemptId),
                    IsSubmitted = r.SubmittedAtUtc != null,
                    StartedAtUtc = r.StartedAtUtc,
                    Score = r.Score,
                    MaxScore = r.MaxScore
                }).ToList()
            };
        }

        public async Task<string> StartOrResumeAsync(string studentId, string quizId, CancellationToken ct)
        {
            var nowUtc = DateTime.UtcNow;

            // Safety: Student must be member + quiz must be active in time window
            var isActive = await _quizRepository.IsQuizActiveForStudentAsync(quizId, studentId, nowUtc, ct);
            if (!isActive)
                throw new InvalidOperationException("Quiz is not active or you have no access.");

            // Resume open attempt
            var openAttemptId = await _quizRepository.GetOpenAttemptIdAsync(quizId, studentId, ct);
            if (!string.IsNullOrWhiteSpace(openAttemptId))
                return openAttemptId;

            // Create new attempt
            return await _quizRepository.CreateAttemptAsync(quizId, studentId, nowUtc, ct);
        }
    }
}
