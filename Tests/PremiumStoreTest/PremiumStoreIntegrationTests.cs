using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Tests.Support;
using Xunit;

public class PremiumStoreIntegrationTests : IClassFixture<ApiFactory>
{
    private readonly HttpClient _client;

    public PremiumStoreIntegrationTests(ApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetRewards_ReturnsRewardsList()
    {
        var response = await _client.GetAsync("/Store/Get");

        response.EnsureSuccessStatusCode();
        var rewards = await response.Content.ReadFromJsonAsync<JsonElement>();
        Xunit.Assert.Equal(JsonValueKind.Array, rewards.ValueKind);
    }

    [Fact]
    public async Task BuyReward_ForMissingReward_ReturnsConflict()
    {
        var response = await _client.PostAsync("/Store/BuyReward?rewardId=999", null);

        Xunit.Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }
}
