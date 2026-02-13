using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quizle.Infrastructure.Data.Entities;

namespace Quizle.Infrastructure.Data.Configurations
{
    public class ChoiceOptionConfigurations : IEntityTypeConfiguration<ChoiceOption>
    {
        public void Configure(EntityTypeBuilder<ChoiceOption> builder)
        {
            builder
                .HasOne(o => o.Question)
                .WithMany(q => q.Options)
                .HasForeignKey(o => o.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
