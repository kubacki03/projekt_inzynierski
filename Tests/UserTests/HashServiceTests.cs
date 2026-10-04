using Xunit;
using projekt_inzynierski.Server.Users.Infrastructures.Services;
using projekt_inzynierski.Server.Users.Domain.Models;

public class HashServiceTests
{
    [Fact]
    public void HashPassword_ReturnsHashedString()
    {
        // Arrange
        var hashService = new HashService();
        var password = "myPassword";

        // Act
        var hash = hashService.HashPassword(password);

        // Assert
        Xunit.Assert.False(string.IsNullOrEmpty(hash));
        Xunit.Assert.NotEqual(password, hash);
    }
}
