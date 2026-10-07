using System.Net.Http.Json;
using System.Text.Json;
using Tests.Support;
using Xunit;

public class AchievementsIntegrationTests : IClassFixture<ApiFactory>
{
    private readonly HttpClient _client;

    public AchievementsIntegrationTests(ApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetUserAchievements_ReturnsAchievementsList()
    {
        var response = await _client.GetAsync("/UserAchievment/GetUserAchievments");

        response.EnsureSuccessStatusCode();
        var achievements = await response.Content.ReadFromJsonAsync<JsonElement>();
        Xunit.Assert.Equal(JsonValueKind.Array, achievements.ValueKind);
    }

    [Fact]
    public async Task GetMostActiveUsers_ReturnsUserIds()
    {
        var response = await _client.GetAsync("/UserAchievment/GetMostActiveUsers");

        response.EnsureSuccessStatusCode();
        var nicknames = await response.Content.ReadFromJsonAsync<List<string>>();
        Xunit.Assert.NotNull(nicknames);
        Xunit.Assert.True(nicknames.Count <= 5);
    }
}
