using Quizle.Core.Contracts;
using Quizle.Core.Dtos;
using Quizle.Core.Entities;
using Quizle.Core.Types;

namespace Quizle.Core.Services
{
    public class StudentAttemptService : IStudentAttemptService
    {
        private readonly IQuizRepository _quizRepository;
        private readonly ISchoolGroupRepository _groupRepository;
        private readonly IQuizAttemptRepository _attemptRepository;

        public StudentAttemptService(
            IQuizRepository quizRepository,
            ISchoolGroupRepository groupRepository,
            IQuizAttemptRepository attemptRepository)
        {
            _quizRepository = quizRepository;
            _groupRepository = groupRepository;
            _attemptRepository = attemptRepository;
        }

        public async Task<SolveQuestionDto> GetSolveVmAsync(string attemptId, int index, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<QuizAttempt> StartOrGetAttemptAsync(string quizId, string studentId, CancellationToken ct)
        {
            var quiz = await _quizRepository.GetReadOnlyByIdAsync(quizId, ct);

            if (quiz == null)
                throw new InvalidOperationException("Quiz not found.");

            if (quiz.Status != QuizStatus.Active)
                throw new InvalidOperationException("Quiz is not active.");

            if (!quiz.ActiveUntilUtc.HasValue)
                throw new InvalidOperationException("Quiz active window is not configured.");

            if (DateTime.UtcNow > quiz.ActiveUntilUtc.Value)
                throw new InvalidOperationException("Quiz has ended.");

            // validate student is in group
            var inGroup = await _groupRepository
                .IsStudentInGroupAsync(quiz.SchoolGroupId, studentId, ct);

            if (!inGroup) throw new InvalidOperationException("You are not in this group.");

            // enforce 1 attempt: get existing or create
            var attempt = await _attemptRepository.GetAsync(quizId, studentId, ct);
            
            if (attempt != null) return attempt;

            attempt = new QuizAttempt
            {
                QuizId = quizId,
                StudentId = studentId,
                StartedAtUtc = DateTime.UtcNow
            };

            await _attemptRepository.CreateAsync(attempt);

            return attempt;
        }
    }
}
