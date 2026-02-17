using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quizle.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizle.Infrastructure.Data.Configurations
{
    public class SchoolGroupConfigurations : IEntityTypeConfiguration<SchoolGroup>
    {
        public void Configure(EntityTypeBuilder<SchoolGroup> builder)
        {
            builder
                .HasOne(g => g.Teacher)
                .WithMany()
                .HasForeignKey(g => g.TeacherId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasMany(g => g.Students)
                .WithMany(u => u.GroupStudents)
                .UsingEntity<Dictionary<string, object>>(
                    "GroupStudent",
                    r => r.HasOne<ApplicationUser>()
                          .WithMany()
                          .HasForeignKey("StudentId")
                          .OnDelete(DeleteBehavior.NoAction),
                    l => l.HasOne<SchoolGroup>()
                          .WithMany()
                          .HasForeignKey("GroupId")
                          .OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.HasKey("GroupId", "StudentId");
                        j.HasIndex("GroupId");
                        j.HasIndex("StudentId");
                        j.ToTable("GroupStudents");
                    });
        }
    }
}
