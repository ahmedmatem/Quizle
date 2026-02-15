using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quizle.Core.Contracts;
using Quizle.Core.Dtos;
using Quizle.Web.Areas.Student.Models;
using System.Security.Claims;

namespace Quizle.Web.Areas.Student.Controllers
{
    [Area("Student")]
    [Authorize(Roles = "Student")]
    public class AttemptsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IStudentAttemptService _service;

        public AttemptsController(
            IMapper mapper,
            IStudentAttemptService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Start(string quizId, CancellationToken ct)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            try
            {
                var attempt = await _service.StartOrGetAttemptAsync(quizId, userId, ct);
                return RedirectToAction(nameof(Solve), new { attemptId = attempt.Id, index = 0 });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index", "Dashboard", new { area = "Student" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Solve(string attemptId, int index = 0, CancellationToken ct = default)
        {
            var solveDto = await _service.GetSolveAsync(attemptId, index, ct);
            var vm = _mapper.Map<SolveQuestionVm>(solveDto);
            return View(vm);
        }

        // AJAX autosave
        [HttpPost]
        public async Task<IActionResult> SaveAnswer([FromBody] SaveAnswerReq req, CancellationToken ct)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var saDto = _mapper.Map<SaveAnswerDto>(req);
            try
            {
                await _service.SaveAnswerAsync(userId, saDto, ct);
                return Ok(new { ok = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ok = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Submit(string attemptId, CancellationToken ct)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            try
            {
                await _service.SubmitAsync(userId, attemptId, ct);
                return RedirectToAction("Index", "Dashboard");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Solve), new { attemptId, index = 0 });
            }
        }
    }
}
