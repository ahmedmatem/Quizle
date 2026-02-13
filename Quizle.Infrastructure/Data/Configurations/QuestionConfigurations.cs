using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quizle.Core.Entities;

namespace Quizle.Infrastructure.Data.Configurations
{
    public class QuestionConfigurations : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder
                .HasOne(q => q.CorrectOption)
                .WithMany() // няма обратна навигация (важно)
                .HasForeignKey(q => q.CorrectOptionId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(x => x.IsDeleted);
        }
    }
}
