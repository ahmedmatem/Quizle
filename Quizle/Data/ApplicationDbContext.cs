using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Quizle.Data.Entities;

namespace Quizle.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<SchoolGroup> SchoolGroups { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<ChoiceOption> ChoiceOptions { get; set; }
        public DbSet<QuizAttempt> QuizAttempts { get; set; }
        public DbSet<StudentAnswer> StudentAnswers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<SchoolGroup>()
                .HasMany(g => g.Students)
                .WithMany(u => u.GroupStudents)
                .UsingEntity<Dictionary<string, object>>(
                    "GroupStudent",
                    r => r.HasOne<ApplicationUser>()
                          .WithMany()
                          .HasForeignKey("StudentId")
                          .OnDelete(DeleteBehavior.Cascade),
                    l => l.HasOne<SchoolGroup>()
                          .WithMany()
                          .HasForeignKey("GroupId")
                          .OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.HasKey("GroupId", "StudentId");
                        j.HasIndex("StudentId");
                        j.ToTable("GroupStudents");
                    });

            builder.Entity<Quiz>()
                .HasMany(qz => qz.Questions)
                .WithMany(q => q.Quizzes)
                .UsingEntity<Dictionary<string, object>>(
                    "QuizQuestion",
                    r => r.HasOne<Question>()
                          .WithMany()
                          .HasForeignKey("QuestionId")
                          .OnDelete(DeleteBehavior.Cascade),
                    
                    l => l.HasOne<Quiz>()
                          .WithMany()
                          .HasForeignKey("QuizId")
                          .OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.HasKey("QuizId", "QuestionId");
                        j.HasIndex("QuizId");
                        j.ToTable("QuizQuestions");
                    });
        }
    }
}
