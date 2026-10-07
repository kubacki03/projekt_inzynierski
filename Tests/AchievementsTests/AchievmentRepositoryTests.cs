using Xunit;
using Microsoft.EntityFrameworkCore;
using projekt_inzynierski.Server.Achievments.Infrastructures.Persistance;
using projekt_inzynierski.Server.Achievments.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class AchievmentRepositoryTests
{
    private AchievementsDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AchievementsDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AchievementsDbContext(options);
    }

    private static Achievement NewAchievement(string id) => new Achievement
    {
        Id = id,
        Name = "Achievement " + id,
        Description = "Description",
        RuleKey = "rule",
        BadgeType = "badge",
        SkillCategory = "category"
    };

    [Fact]
    public void GetMostActiveUsersId_ReturnsTopUsers()
    {
        // Arrange
        var context = GetDbContext();
        var userId1 = Guid.NewGuid();
        var userId2 = Guid.NewGuid();

        context.UserAchievements.AddRange(
            new UserAchievement { UserId = userId1, AchievementId = "1" },
            new UserAchievement { UserId = userId1, AchievementId = "2" },
            new UserAchievement { UserId = userId2, AchievementId = "1" }
        );
        context.SaveChanges();

        var repo = new AchievmentRepository(context);

        // Act
        var result = repo.GetMostActiveUsersId();

        // Assert
        Xunit.Assert.Contains(userId1.ToString(), result);
        Xunit.Assert.Contains(userId2.ToString(), result);
        Xunit.Assert.True(result.Count <= 5);
    }

    [Fact]
    public void GetUserAchievments_ReturnsUserAchievements()
    {
        // Arrange
        var context = GetDbContext();
        var userId = Guid.NewGuid();
        context.Achievements.Add(NewAchievement("1"));
        context.UserAchievements.Add(new UserAchievement { UserId = userId, AchievementId = "1" });
        context.SaveChanges();

        var repo = new AchievmentRepository(context);

        // Act
        var result = repo.GetUserAchievments(userId.ToString());

        // Assert
        Xunit.Assert.Single(result);
        Xunit.Assert.Equal(userId, result[0].UserId);
    }

    [Fact]
    public async Task SaveUserAchievement_AddsAchievement()
    {
        // Arrange
        var context = GetDbContext();
        var repo = new AchievmentRepository(context);
        var userAchievement = new UserAchievement { UserId = Guid.NewGuid(), AchievementId = "1" };

        // Act
        await repo.SaveUserAchievement(userAchievement);

        // Assert
        Xunit.Assert.Single(context.UserAchievements);
    }

    [Fact]
    public async Task GetCompletedAchievementPercentage_ReturnsCorrectValue()
    {
        // Arrange
        var context = GetDbContext();
        var userId = Guid.NewGuid();
        context.Achievements.Add(NewAchievement("1"));
        context.Achievements.Add(NewAchievement("2"));
        context.UserAchievements.Add(new UserAchievement { UserId = userId, AchievementId = "1" });
        context.SaveChanges();

        var repo = new AchievmentRepository(context);

        // Act
        var percent = await repo.GetCompletedAchievementPercentage(userId.ToString());

        // Assert
        Xunit.Assert.Equal(0.5f, percent);
    }
}

