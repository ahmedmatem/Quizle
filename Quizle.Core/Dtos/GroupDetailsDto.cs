namespace Quizle.Core.Dtos
{
    public class GroupDetailsDto
    {
        public string Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? Description { get; set; }

        public List<StudentRowDto> Students { get; set; } = new();

        // form field
        public string? AddStudentEmail { get; set; }
    }

    public class StudentRowDto
    {
        public string Id { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string? FullName { get; set; }
    }
}
