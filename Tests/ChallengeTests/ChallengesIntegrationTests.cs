using System.Net.Http.Json;
using System.Text.Json;
using Tests.Support;
using Xunit;

public class ChallengesIntegrationTests : IClassFixture<ApiFactory>
{
    private readonly HttpClient _client;

    public ChallengesIntegrationTests(ApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetUserChallenges_ReturnsChallengeList()
    {
        var response = await _client.GetAsync("/UserChallenge/Get");

        response.EnsureSuccessStatusCode();
        var challenges = await response.Content.ReadFromJsonAsync<JsonElement>();
        Xunit.Assert.Equal(JsonValueKind.Array, challenges.ValueKind);
    }

    [Fact]
    public async Task GetUserBadges_ReturnsOk()
    {
        var response = await _client.GetAsync("/UserChallenge/GetBadges");

        response.EnsureSuccessStatusCode();
    }
}
