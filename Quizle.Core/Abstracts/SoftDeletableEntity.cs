
namespace Quizle.Core.Abstracts
{
    public abstract class SoftDeletableEntity : ISoftDeletable
    {
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOnUtc { get; set; }
    }
}
