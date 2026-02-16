namespace Quizle.Web.Areas.Teacher.Models
{
    public class GroupDetailsVm
    {
        public string Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? Description { get; set; }

        public List<StudentRowVm> Students { get; set; } = new();

        // form field
        public string? AddStudentEmail { get; set; }
    }

    public class StudentRowVm
    {
        public string Id { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string? FullName { get; set; }
    }
}
