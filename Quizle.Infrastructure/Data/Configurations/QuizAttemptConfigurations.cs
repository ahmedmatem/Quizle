using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quizle.Infrastructure.Data.Entities;

namespace Quizle.Infrastructure.Data.Configurations
{
    public class QuizAttemptConfigurations : IEntityTypeConfiguration<QuizAttempt>
    {
        public void Configure(EntityTypeBuilder<QuizAttempt> builder)
        {
            builder
                .HasIndex(a => new { a.QuizId, a.StudentId })
                .IsUnique();
        }
    }
}
