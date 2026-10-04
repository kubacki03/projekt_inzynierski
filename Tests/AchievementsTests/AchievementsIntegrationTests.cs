using Xunit;
using System.Threading.Tasks;
using System.Net.Http.Json;

using projekt_inzynierski.Server;
using projekt_inzynierski.Server.Achievments.Domain.Models;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.AspNetCore.Mvc.Testing;

public class AchievementsIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public AchievementsIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetUserAchievements_ReturnsAchievementsList()
    {
        // Act
        var response = await _client.GetAsync("/api/achievements/user/user1");

        // Assert
        response.EnsureSuccessStatusCode();
        var achievements = await response.Content.ReadFromJsonAsync<List<UserAchievement>>();
        Xunit.Assert.NotNull(achievements);
        Xunit.Assert.True(achievements.Count >= 0);
    }

    [Fact]
    public async Task GetMostActiveUsers_ReturnsUserIds()
    {
        // Act
        var response = await _client.GetAsync("/api/achievements/most-active");

        // Assert
        response.EnsureSuccessStatusCode();
        var userIds = await response.Content.ReadFromJsonAsync<List<string>>();
        Xunit.Assert.NotNull(userIds);
        Xunit.Assert.True(userIds.Count <= 5);
    }
}

