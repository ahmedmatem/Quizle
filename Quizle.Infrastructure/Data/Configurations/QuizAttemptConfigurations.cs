using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quizle.Core.Entities;

namespace Quizle.Infrastructure.Data.Configurations
{
    public class QuizAttemptConfigurations : IEntityTypeConfiguration<QuizAttempt>
    {
        public void Configure(EntityTypeBuilder<QuizAttempt> builder)
        {
            builder
                .HasIndex(a => new { a.QuizId, a.StudentId })
                .IsUnique();

            builder
                .HasOne(a => a.Student)
                .WithMany(u => u.QuizAttempts)
                .HasForeignKey(a => a.StudentId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
