using Microsoft.Extensions.Options;
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
        private readonly IQuestionRepository _questionRepository;
        private readonly IStudentAnswerRepository _studentAnswerRepository;

        public StudentAttemptService(
            IQuizRepository quizRepository,
            ISchoolGroupRepository groupRepository,
            IQuizAttemptRepository attemptRepository,
            IQuestionRepository questionRepository,
            IStudentAnswerRepository studentAnswerRepository)
        {
            _quizRepository = quizRepository;
            _groupRepository = groupRepository;
            _attemptRepository = attemptRepository;
            _questionRepository = questionRepository;
            _studentAnswerRepository = studentAnswerRepository;
        }

        public async Task<SolveQuestionDto> GetSolveVmAsync(string attemptId, int index, CancellationToken ct)
        {
            var attempt = await _attemptRepository.GetWithQuizAsync(attemptId, ct);

            if (attempt == null) 
                throw new InvalidOperationException("Attempt not found.");
            if (attempt.SubmittedAtUtc != null) 
                throw new InvalidOperationException("Attempt already submitted.");

            var quiz = attempt.Quiz;

            if (quiz.Status != QuizStatus.Active)
                throw new InvalidOperationException("Quiz is not active.");

            if (!quiz.ActiveUntilUtc.HasValue)
                throw new InvalidOperationException("Quiz has no end time.");

            // Load ordered question ids
            var ordered = await _questionRepository.GetOrderedQuestionsAsync(quiz.Id, ct);
            
            if (ordered.Count == 0) throw new InvalidOperationException("Quiz has no questions.");

            if (index < 0) index = 0;
            if (index >= ordered.Count) index = ordered.Count - 1;

            var qid = ordered[index];

            var question = await _questionRepository.GetWithOptionsAsync(qid, ct);

            var existingAnswer = await _studentAnswerRepository.ExistingAnswerAsync(attempt.Id, qid, ct);
            
            return new SolveQuestionDto
            {
                AttemptId = attempt.Id,
                QuizId = quiz.Id,
                QuizTitle = quiz.Title,
                Index = index,
                Total = ordered.Count,
                StartedAtUtc = attempt.StartedAtUtc,
                ActiveUntilUtc = quiz.ActiveUntilUtc.Value,
                QuestionId = question.Id,
                Text = question.Text,
                Type = question.Type,
                Points = question.Points,
                Options = question.Options
                    .Select(o => new OptionDto { Id = o.Id, Text = o.Text })
                    .ToList(),
                SelectedOptionId = existingAnswer?.SelectedOptionId,
                NumericValue = existingAnswer?.NumericValue,
                TextValue = existingAnswer?.TextValue
            };
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

            await _attemptRepository.CreateAsync(attempt, ct);

            return attempt;
        }
    }
}
