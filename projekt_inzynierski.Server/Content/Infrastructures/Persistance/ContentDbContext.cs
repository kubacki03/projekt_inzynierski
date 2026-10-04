using Microsoft.EntityFrameworkCore;
using projekt_inzynierski.Server.Content.Domain.Models;
using projekt_inzynierski.Server.Courses.Domain.Models;

namespace projekt_inzynierski.Server.Content.Infrastructures.Persistance
{
    public class ContentDbContext : DbContext
    {
        public ContentDbContext(DbContextOptions<ContentDbContext> options)
           : base(options)
        {
        }

        public DbSet<Answer> Answers { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Theory> Theories { get; set; }
        public DbSet<Subject> Subjects { get; set; }
    
        public DbSet<UserExerciseFeedback> UserExerciseFeedback { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Exercise>()
                .HasIndex(x => x.SubjectId);

            modelBuilder.Entity<Theory>()
                .HasIndex(x => x.SubjectId);

            modelBuilder.Entity<Quiz>()
                .HasIndex(x => x.SubjectId);
             
            modelBuilder.Entity<Quiz>()
                .HasOne(q => q.Subject)
                .WithMany(s => s.Quizzes)
                .HasForeignKey(q => q.SubjectId);

            modelBuilder.Entity<Theory>()
                .HasOne(t => t.Subject)
                .WithMany(s => s.Theories)
                .HasForeignKey(t => t.SubjectId);

            modelBuilder.Entity<Exercise>()
                .HasOne(e => e.Subject)
                .WithMany(s => s.Exercises)
                .HasForeignKey(e => e.SubjectId);
             
            modelBuilder.Entity<UserExerciseFeedback>()
                .HasOne(e => e.Exercise)
                .WithMany(s => s.Feedbacks)
                .HasForeignKey(e => e.ExerciesId);
        } 
    }
}
