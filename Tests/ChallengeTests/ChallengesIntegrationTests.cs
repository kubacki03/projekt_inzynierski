using Xunit;
using System.Threading.Tasks;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using projekt_inzynierski.Server;
using projekt_inzynierski.Server.Challenges.Domain.Models;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using projekt_inzynierski.Server.Challenges.Application.DTOs;

public class ChallengesIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ChallengesIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetUserChallenges_ReturnsChallengeList()
    {
        // Act
        var response = await _client.GetAsync("/api/challenges/user/user1");

        // Assert
        response.EnsureSuccessStatusCode();
        var challenges = await response.Content.ReadFromJsonAsync<List<UserChallengeDto>>();
        Xunit.Assert.NotNull(challenges);
        Xunit.Assert.True(challenges.Count >= 0); 
    }

    [Fact]
    public async Task AddUserChallenge_ReturnsSuccess()
    {
        // Act
        var response = await _client.PostAsync("/api/challenges/user/user1", null);

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync();
        Xunit.Assert.Contains("success", result.ToLower()); 
    }
}
