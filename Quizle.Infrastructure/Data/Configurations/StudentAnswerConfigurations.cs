using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quizle.Core.Entities;

namespace Quizle.Infrastructure.Data.Configurations
{
    public class StudentAnswerConfigurations : IEntityTypeConfiguration<StudentAnswer>
    {
        public void Configure(EntityTypeBuilder<StudentAnswer> builder)
        {
            builder
                .HasOne(sa => sa.SelectedOption)
                .WithMany() // няма нужда от обратна навигация
                .HasForeignKey(sa => sa.SelectedOptionId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasIndex(sa => new { sa.QuizAttemptId, sa.QuestionId })
                .IsUnique();
            builder
                .HasIndex(sa => sa.QuizAttemptId);
        }
    }
}
