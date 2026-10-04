using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using projekt_inzynierski.Server;
using Microsoft.AspNetCore.Mvc.Testing;
using projekt_inzynierski.Server.Users.Application.DTOs;
using projekt_inzynierski.Server.Users.Application.Commands;
using Microsoft.VisualStudio.TestPlatform.TestHost;

public class UsersLoginIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public UsersLoginIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task LoginUser_ReturnsJwtToken()
    { 
        var registerDto = new UserRegisterDto
        {
            email = "loginuser@example.com",
            password = "Test1234!",
            firstName = "Anna",
            nickname = "ania",
            birthDate = new DateTime(1995, 5, 5),
            educationLevel = "Œrednie",
            experience = "Pocz¹tkuj¹cy",
            gender = "K"
        };
        await _client.PostAsJsonAsync("/api/users/register", new RegisterUserCommand(registerDto));
         
        var loginDto = new
        {
            email = "loginuser@example.com",
            password = "Test1234!"
        };

        var response = await _client.PostAsJsonAsync("/api/users/login", loginDto);

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
        Xunit.Assert.False(string.IsNullOrEmpty(result?.Token));
    }

    public class LoginResponseDto
    {
        public string Token { get; set; }
    }
}
