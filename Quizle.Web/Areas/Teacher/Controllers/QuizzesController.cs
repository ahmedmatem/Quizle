using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quizle.Core.Contracts;
using Quizle.Core.Dtos;
using Quizle.Core.Types;
using Quizle.Web.Areas.Teacher.Models;
using System.Security.Claims;

namespace Quizle.Web.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    [Authorize(Roles = "Teacher")]
    public class QuizzesController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ITeacherQuizService _service;

        private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        public QuizzesController(
            IMapper mapper,
            ITeacherQuizService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string groupId, CancellationToken ct)
        {
            var items = await _service.GetQuizzesForGroupAsync(UserId, groupId, ct);
            ViewBag.GroupId = groupId;
            var vm = _mapper.Map<List<QuizListItemVm>>(items);
            return View(vm);
        }

        [HttpGet]
        public IActionResult Create(string groupId)
        => View(new QuizCreateVm { SchoolGroupId = groupId });

        [HttpPost]
        public async Task<IActionResult> Create(QuizCreateVm vm, CancellationToken ct)
        {
            try
            {
                if (!ModelState.IsValid) return View(vm);

                var dto = _mapper.Map<QuizCreateDto>(vm);
                var id = await _service.CreateQuizAsync(UserId, dto, ct);
                return RedirectToAction(nameof(Edit), new { id });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(vm);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id, string? search, QuestionType? type, CancellationToken ct)
        {
            var dto = await _service.GetBuilderAsync(UserId, id, search, type, ct);
            var vm = _mapper.Map<QuizBuilderVm>(dto);
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddQuestion(string quizId, string questionId, CancellationToken ct)
        {
            try { 
                await _service.AddQuestionAsync(UserId, quizId, questionId, ct); 
            }
            catch (Exception ex) { TempData["Error"] = ex.Message; }

            return RedirectToAction(nameof(Edit), new { id = quizId });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveQuestion(string quizId, string questionId, CancellationToken ct)
        {
            try {
                await _service.RemoveQuestionAsync(UserId, quizId, questionId, ct); 
            }
            catch (Exception ex) { TempData["Error"] = ex.Message; }

            return RedirectToAction(nameof(Edit), new { id = quizId });
        }

        [HttpPost]
        public async Task<IActionResult> MoveUp(string quizId, string questionId, CancellationToken ct)
        {
            try { 
                await _service.MoveAsync(UserId, quizId, questionId, -1, ct); 
            }
            catch (Exception ex) { TempData["Error"] = ex.Message; }

            return RedirectToAction(nameof(Edit), new { id = quizId });
        }

        [HttpPost]
        public async Task<IActionResult> MoveDown(string quizId, string questionId, CancellationToken ct)
        {
            try { 
                await _service.MoveAsync(UserId, quizId, questionId, +1, ct);
            }
            catch (Exception ex) { TempData["Error"] = ex.Message; }

            return RedirectToAction(nameof(Edit), new { id = quizId });
        }
    }
}
