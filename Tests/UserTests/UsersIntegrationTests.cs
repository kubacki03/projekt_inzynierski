using System.Net.Http.Json;
using System.Text.Json;
using projekt_inzynierski.Server.Users.Application.DTOs;
using Tests.Support;
using Xunit;

public class UsersIntegrationTests : IClassFixture<ApiFactory>
{
    private readonly HttpClient _client;

    public UsersIntegrationTests(ApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task RegisterUser_LogsInAndReturnsRole()
    {
        var registerDto = new UserRegisterDto
        {
            email = "testuser@example.com",
            password = "Test1234!",
            firstName = "Jan",
            nickname = "janek",
            birthDate = new DateTime(2000, 1, 1),
            educationLevel = "Higher",
            experience = "Intermediate",
            gender = "M"
        };

        var response = await _client.PostAsJsonAsync("/User/Register", registerDto);

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<JsonElement>();
        Xunit.Assert.Equal("user", result.GetProperty("role").GetString());
        Xunit.Assert.Contains(response.Headers, h => h.Key == "Set-Cookie");
    }
}
