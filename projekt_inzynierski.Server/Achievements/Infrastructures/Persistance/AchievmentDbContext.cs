using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using projekt_inzynierski.Server.Achievments.Domain.Models;

public class AchievementsDbContext : DbContext
{
    public AchievementsDbContext(DbContextOptions<AchievementsDbContext> options)
        : base(options) { }

  
    public DbSet<Achievement> Achievements => Set<Achievement>();
    public DbSet<UserAchievement> UserAchievements => Set<UserAchievement>();
    public DbSet<UserProgress> UserProgress => Set<UserProgress>();
    public DbSet<StudyDay> StudyDays => Set<StudyDay>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      
        modelBuilder.Entity<Achievement>(b =>
        {
            b.HasKey(a => a.Id);
            b.Property(a => a.Name).IsRequired().HasMaxLength(200);
            b.Property(a => a.Description).HasMaxLength(500);
            b.Property(a => a.RuleKey).IsRequired().HasMaxLength(100);
            b.Property(a => a.RuleConfigJson).HasDefaultValue("{}");
        });

   
        modelBuilder.Entity<UserAchievement>(b =>
        {
            b.HasKey(ua => new { ua.UserId, ua.AchievementId });
            b.HasOne(ua => ua.Achievement)
                .WithMany()
                .HasForeignKey(ua => ua.AchievementId);
            b.Property(ua => ua.EarnedAtUtc)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

    
        modelBuilder.Entity<UserProgress>(b =>
        {
            b.HasKey(up => new { up.UserId, up.LessonId });
            b.Property(up => up.Language)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<StudyDay>(b =>
        {
            b.HasKey(sd => new { sd.UserId, sd.Day });
        });


        modelBuilder.Entity<Achievement>().HasData(
    new Achievement
    {
        Id = "first-lesson",
        Name = "Pierwsza lekcja!",
        Points = 10,
        Description = "Ukończ pierwszą lekcję",
        RuleKey = "LessonCount",
        RuleConfigJson = "{\"Threshold\":1}",
        BadgeType="bronze",
        SkillCategory="common"
    },
    new Achievement
    {
        Id = "5-lessons",
        Name = "Rozkręcamy się",
        Points = 25,
        Description = "Ukończ 5 lekcji",
        RuleKey = "LessonCount",
        RuleConfigJson = "{\"Threshold\":5}",
        BadgeType = "silver",
        SkillCategory = "common"
    },
    new Achievement
    {
        Id = "7-day-streak",
        Name = "Szczwany Streak",
        Points = 40,
        Description = "Ucz się 7 dni z rzędu",
        RuleKey = "Streak",
        RuleConfigJson = "{\"Days\":7}",
        BadgeType = "silver",
        SkillCategory = "common"
    },
    new Achievement
    {
        Id = "polyglot",
        Name = "Poliglota kodu",
        Points = 30,
        Description = "Ukończ lekcje w 2 różnych językach",
        RuleKey = "LanguageExplorer",
        RuleConfigJson = "{\"DistinctLanguages\":2}",
        BadgeType = "gold",
        SkillCategory = "common"
    },
     new Achievement
     {
         Id = "task-1",
         Name = "Pierwsze zadanie!",
         Description = "Ukończ swoje pierwsze zadanie.",
         Points = 10,
         RuleKey = "task-count",
         RuleConfigJson = JsonSerializer.Serialize(new { Threshold = 1 }),
         BadgeType = "bronze",
         SkillCategory = "common"
     },
        new Achievement
        {
            Id = "task-10",
            Name = "10 zadań zaliczonych",
            Description = "Ukończ 10 zadań.",
            Points = 50,
            RuleKey = "task-count",
            RuleConfigJson = JsonSerializer.Serialize(new { Threshold = 10 }),
            BadgeType = "gold",
            SkillCategory = "common"
        },
        new Achievement
        {
            Id = "quiz-90",
            Name = "Mistrz quizów",
            Description = "Zdobądź minimum 90% w quizie.",
            Points = 100,
            RuleKey = "quiz-score",
            RuleConfigJson = JsonSerializer.Serialize(new { MinScore = 90 }),
            BadgeType = "silver",
            SkillCategory = "common"
        }
);


    }
}
