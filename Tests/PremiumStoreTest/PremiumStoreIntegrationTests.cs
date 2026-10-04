using Xunit;
using System.Threading.Tasks;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using projekt_inzynierski.Server;
using projekt_inzynierski.Server.PremiumStore.Domain.Models;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestPlatform.TestHost;

public class PremiumStoreIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public PremiumStoreIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetRewards_ReturnsRewardsList()
    {
        // Act
        var response = await _client.GetAsync("/api/store/rewards");

        // Assert
        response.EnsureSuccessStatusCode();
        var rewards = await response.Content.ReadFromJsonAsync<List<Reward>>();
        Xunit.Assert.NotNull(rewards);
        Xunit.Assert.True(rewards.Count >= 0); 
    }

    [Fact]
    public async Task RedeemReward_ReturnsSuccessOrFailure()
    { 
        // Act
        var redeemRequest = new
        {
            userId = "user1",
            rewardId = 1
        };

        var response = await _client.PostAsJsonAsync("/api/store/redeem", redeemRequest);

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync();
        Xunit.Assert.Contains("success", result.ToLower()); 
    }
}
