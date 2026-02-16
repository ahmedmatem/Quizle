namespace Quizle.Web.Areas.Teacher.Models
{
    public class GroupListItemVm
    {
        public string Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public int StudentsCount { get; set; }
        public int QuizzesCount { get; set; }
    }
}
