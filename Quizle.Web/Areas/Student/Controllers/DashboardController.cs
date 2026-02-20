using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quizle.Core.Contracts;
using Quizle.Web.Areas.Student.Models;
using System.Security.Claims;

namespace Quizle.Web.Areas.Student.Controllers;

[Area("Student")]
[Authorize(Roles = "Student")]
public class DashboardController : Controller
{
    private readonly IMapper _mapper;
    private readonly IStudentQuizService _service;

    public DashboardController(
        IMapper mapper,
        IStudentQuizService service)
    {
        _mapper = mapper;
        _service = service;
    }

    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var dto = await _service.GetDashboardAsync(UserId, ct);
        var vm = _mapper.Map<StudentDashboardVm>(dto);
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Start(string quizId, CancellationToken ct)
    {
        try
        {
            var attemptId = await _service.StartOrResumeAsync(UserId, quizId, ct);
            return RedirectToAction("Solve", "Attempts", new { area = "Student", attemptId });
        }
        catch (Exception ex)
        {
            var dto = await _service.GetDashboardAsync(UserId, ct);
            var vm = _mapper.Map<StudentDashboardVm>(dto);
            vm.Error = ex.Message;
            return View("Index", vm);
        }
    }
}
