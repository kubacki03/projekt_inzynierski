using System.Net.Http.Json;
using System.Text.Json;
using Tests.Support;
using Xunit;

public class AiHelperIntegrationTests : IClassFixture<ApiFactory>
{
    private readonly HttpClient _client;

    public AiHelperIntegrationTests(ApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task PostChatMessage_ReturnsAiResponse()
    {
        var response = await _client.PostAsJsonAsync("/AssistantChat/GetChatResponse", new { message = "Hello, AI!" });

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<JsonElement>();
        Xunit.Assert.True(result.GetProperty("success").GetBoolean());
        Xunit.Assert.Equal("stubbed AI answer", result.GetProperty("message").GetString());
    }
}
