using Xunit;
using System.Threading.Tasks;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using projekt_inzynierski.Server;
using Microsoft.VisualStudio.TestPlatform.TestHost;

public class AiHelperIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public AiHelperIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task PostChatMessage_ReturnsAiResponse()
    {
        // Arrange
        var request = new { message = "Hello, AI!" };

        // Act
        var response = await _client.PostAsJsonAsync("/api/ai/chat", request);

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync();
        Xunit.Assert.False(string.IsNullOrEmpty(result)); 
    }
}
