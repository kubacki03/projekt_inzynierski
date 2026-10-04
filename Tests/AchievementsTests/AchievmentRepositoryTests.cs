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

    [Fact]
    public void GetMostActiveUsersId_ReturnsTopUsers()
    {
        // Arrange
        var context = GetDbContext();
        var userId1 = Guid.NewGuid();
        var userId2 = Guid.NewGuid();

        context.UserAchievements.AddRange(
            new UserAchievement { UserId = userId1 },
            new UserAchievement { UserId = userId1 },
            new UserAchievement { UserId = userId2 }
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
        context.UserAchievements.Add(new UserAchievement { UserId = userId });
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
        var userAchievement = new UserAchievement { UserId = Guid.NewGuid() };

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
        context.Achievements.Add(new Achievement { Id = "1", Name = "A" });
        context.Achievements.Add(new Achievement { Id = "2", Name = "B" });
        context.UserAchievements.Add(new UserAchievement { UserId = userId });
        context.SaveChanges();

        var repo = new AchievmentRepository(context);

        // Act
        var percent = await repo.GetCompletedAchievementPercentage(userId.ToString());

        // Assert
        Xunit.Assert.Equal(0.5f, percent);
    }
}

