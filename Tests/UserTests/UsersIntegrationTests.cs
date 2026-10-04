using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using projekt_inzynierski.Server;
using Microsoft.AspNetCore.Mvc.Testing;
using projekt_inzynierski.Server.Users.Application.Commands;
using projekt_inzynierski.Server.Users.Application.DTOs;
using Microsoft.VisualStudio.TestPlatform.TestHost;

public class UsersIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public UsersIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task RegisterUser_ReturnsPublicId()
    {
        // Arrange
        var registerDto = new UserRegisterDto
        {
            email = "testuser@example.com",
            password = "Test1234!",
            firstName = "Jan",
            nickname = "janek",
            birthDate = new DateTime(2000, 1, 1),
            educationLevel = "Wy¿sze",
            experience = "Œrednie",
            gender = "M"
        };

        var command = new RegisterUserCommand(registerDto);

        // Act
        var response = await _client.PostAsJsonAsync("/api/users/register", command);

        // Assert
        response.EnsureSuccessStatusCode();
        var publicId = await response.Content.ReadFromJsonAsync<Guid>();
        Xunit.Assert.NotEqual(Guid.Empty, publicId);
    }
}
