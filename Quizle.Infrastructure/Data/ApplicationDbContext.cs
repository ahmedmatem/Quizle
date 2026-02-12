using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Quizle.Infrastructure.Data.Entities;

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
        public DbSet<QuizQuestion> QuizQuestions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // StudentAnswer -> SelectedOption (nullable FK)
            builder.Entity<StudentAnswer>()
                .HasOne(sa => sa.SelectedOption)
                .WithMany() // няма нужда от обратна навигация
                .HasForeignKey(sa => sa.SelectedOptionId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<StudentAnswer>()
                .HasIndex(sa => new { sa.QuizAttemptId, sa.QuestionId })
                .IsUnique();
            builder.Entity<StudentAnswer>()
                .HasIndex(sa => sa.QuizAttemptId);

            builder.Entity<QuizAttempt>()
                .HasIndex(a => new { a.QuizId, a.StudentId })
                .IsUnique();

            // Question -> Options (1:N)
            builder.Entity<ChoiceOption>()
                .HasOne(o => o.Question)
                .WithMany(q => q.Options)
                .HasForeignKey(o => o.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Question -> CorrectOption (nullable FK) (separate relationship)
            builder.Entity<Question>()
                .HasOne(q => q.CorrectOption)
                .WithMany() // няма обратна навигация (важно)
                .HasForeignKey(q => q.CorrectOptionId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<SchoolGroup>()
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

            builder.Entity<QuizQuestion>(e =>
            {
                e.ToTable("QuizQuestions");

                e.HasKey(x => new { x.QuizId, x.QuestionId });

                e.HasOne(x => x.Quiz)
                    .WithMany(q => q.QuizQuestions)
                    .HasForeignKey(x => x.QuizId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(x => x.Question)
                    .WithMany(q => q.QuizQuestions)
                    .HasForeignKey(x => x.QuestionId)
                    .OnDelete(DeleteBehavior.NoAction); // safer if Questions are reused

                e.HasIndex(x => new { x.QuizId, x.Order }).IsUnique(); // fast order queries

            });
        }
    }
}
