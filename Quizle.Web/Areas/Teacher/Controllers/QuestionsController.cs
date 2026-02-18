using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quizle.Core.Contracts;
using Quizle.Core.Dtos;
using Quizle.Core.Services;
using Quizle.Core.Types;
using Quizle.Web.Areas.Teacher.Models;
using System.Security.Claims;

namespace Quizle.Web.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    [Authorize(Roles = "Teacher")]
    public class QuestionsController : Controller
    {
        private readonly ITeacherQuestionService _service;
        private readonly IMapper _mapper;

        private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        public QuestionsController(
            ITeacherQuestionService service,
            IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? search, QuestionType? type, CancellationToken ct)
        {
            var dto = await _service.GetIndexAsync(UserId, search, type, ct);
            var vm = _mapper.Map<QuestionIndexVm>(dto);
            return View(vm);
        }

        [HttpGet]
        public IActionResult Create()
        {
            // default MC with 4 options
            var vm = new QuestionEditVm
            {
                Type = QuestionType.MultipleChoice,
                Points = 1,
                Options = new()
            {
                new OptionInputVm{ Text = "" },
                new OptionInputVm{ Text = "" },
                new OptionInputVm{ Text = "" },
                new OptionInputVm{ Text = "" },
            },
                CorrectIndex = 0
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(QuestionEditVm vm, CancellationToken ct)
        {
            try
            {
                if (!ModelState.IsValid) return View(vm);

                var dto = _mapper.Map<QuestionEditDto>(vm);
                var id = await _service.CreateAsync(UserId, dto, ct);
                TempData["Success"] = "Question created.";
                return RedirectToAction(nameof(Edit), new { id });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(vm);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id, CancellationToken ct)
        {
            var dto = await _service.GetForEditAsync(UserId, id, ct);
            var vm = _mapper.Map<QuestionEditVm>(dto);
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(QuestionEditVm vm, CancellationToken ct)
        {
            try
            {
                if (!ModelState.IsValid) return View(vm);

                var dto = _mapper.Map<QuestionEditDto>(vm);
                await _service.UpdateAsync(UserId, dto, ct);
                TempData["Success"] = "Saved.";
                return RedirectToAction(nameof(Edit), new { id = vm.Id });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(vm);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id, CancellationToken ct)
        {
            await _service.SoftDeleteAsync(UserId, id, ct);
            TempData["Success"] = "Deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}
