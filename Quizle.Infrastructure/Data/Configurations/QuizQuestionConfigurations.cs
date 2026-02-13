using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quizle.Core.Entities;

namespace Quizle.Infrastructure.Data.Configurations
{
    public class QuizQuestionConfigurations : IEntityTypeConfiguration<QuizQuestion>
    {
        public void Configure(EntityTypeBuilder<QuizQuestion> builder)
        {
            builder.ToTable("QuizQuestions");

            builder.HasKey(x => new { x.QuizId, x.QuestionId });

            builder.HasOne(x => x.Quiz)
                .WithMany(q => q.QuizQuestions)
                .HasForeignKey(x => x.QuizId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Question)
                .WithMany(q => q.QuizQuestions)
                .HasForeignKey(x => x.QuestionId)
                .OnDelete(DeleteBehavior.NoAction); // safer if Questions are reused

            builder.HasIndex(x => new { x.QuizId, x.Order })
                .IsUnique(); // ensures unique order inside quiz

        }
    }
}
