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
    private readonly IStudentDashboardService _dashboardService;

    public DashboardController(
        IMapper mapper,
        IStudentDashboardService dashboardService)
    {
        _mapper = mapper;
        _dashboardService = dashboardService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var quizzes = await _dashboardService.GetStudentDashboardActiveQuizzesAsync(userId, ct);

        var vm = new StudentDashboardVm();

        vm.ActiveQuizzes = quizzes
                .Select(q => _mapper.Map<ActiveQuizCardVm>(q))
                .ToList();
        vm.Error = TempData["Error"] as string;

        return View(vm);
    }
}
