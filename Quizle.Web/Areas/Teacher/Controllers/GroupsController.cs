using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quizle.Core.Dtos;
using Quizle.Core.Services;
using Quizle.Web.Areas.Teacher.Models;
using System.Security.Claims;

namespace Quizle.Web.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    [Authorize(Roles = "Teacher")]
    public class GroupsController : Controller
    {
        private readonly TeacherGroupService _service;
        private readonly IMapper _mapper;

        private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        public GroupsController(TeacherGroupService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var groups = await _service.GetMyGroupsAsync(UserId, ct);
            var vm = _mapper.Map<List<GroupListItemVm>>(groups);
            return View(vm);
        }

        [HttpGet]
        public IActionResult Create() => View(new CreateGroupVm());

        [HttpPost]
        public async Task<IActionResult> Create(CreateGroupVm vm, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(vm);

            var dto = _mapper.Map<CreateGroupDto>(vm);
            var id = await _service.CreateAsync(UserId, dto, ct);
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id, CancellationToken ct)
        {
            try
            {
                var dto = await _service.GetDetailsAsync(UserId, id, ct);
                var vm = _mapper.Map<GroupDetailsVm>(dto);
                return View(vm);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddStudent(string id, string addStudentEmail, CancellationToken ct)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(addStudentEmail))
                    throw new InvalidOperationException("Email is required.");

                await _service.AddStudentByEmailAsync(UserId, id, addStudentEmail, ct);
                TempData["Success"] = "Student added.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveStudent(string id, string studentId, CancellationToken ct)
        {
            try
            {
                await _service.RemoveStudentAsync(UserId, id, studentId, ct);
                TempData["Success"] = "Student removed.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
