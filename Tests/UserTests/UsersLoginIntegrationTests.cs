using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using projekt_inzynierski.Server.Users.Application.DTOs;
using Tests.Support;
using Xunit;

public class UsersLoginIntegrationTests : IClassFixture<ApiFactory>
{
    private readonly HttpClient _client;

    public UsersLoginIntegrationTests(ApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task LoginUser_AfterRegistration_SetsAccessTokenCookie()
    {
        var registerDto = new UserRegisterDto
        {
            email = "loginuser@example.com",
            password = "Test1234!",
            firstName = "Anna",
            nickname = "ania",
            birthDate = new DateTime(1995, 5, 5),
            educationLevel = "Secondary",
            experience = "Beginner",
            gender = "K"
        };
        (await _client.PostAsJsonAsync("/User/Register", registerDto)).EnsureSuccessStatusCode();

        var response = await _client.PostAsJsonAsync("/User/Login", new { email = registerDto.email, password = registerDto.password });

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<JsonElement>();
        Xunit.Assert.Equal("user", result.GetProperty("role").GetString());
        Xunit.Assert.Contains(response.Headers.GetValues("Set-Cookie"), c => c.StartsWith("access_token="));
    }

    [Fact]
    public async Task LoginUser_WithWrongPassword_ReturnsUnauthorized()
    {
        var response = await _client.PostAsJsonAsync("/User/Login", new { email = "nobody@example.com", password = "wrong" });

        Xunit.Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
