
namespace Quizle.Infrastructure.Data.Abstracts
{
    public abstract class SoftDeletableEntity : ISoftDeletable
    {
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOnUtc { get; set; }
    }
}
