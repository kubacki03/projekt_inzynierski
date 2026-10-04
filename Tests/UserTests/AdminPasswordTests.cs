using Xunit;
using projekt_inzynierski.Server.Users.Infrastructures.Services;
using AdminModel = projekt_inzynierski.Server.Users.Domain.Models.Admin;

public class AdminPasswordTests
{
    private readonly HashService _hashService = new();

    [Fact]
    public void VerifyAdminPassword_AcceptsPbkdf2Hash_AndRejectsWrongPassword()
    {
        var admin = new AdminModel { Login = "a" };
        admin.Password = _hashService.HashAdminPassword("secret", admin);

        Xunit.Assert.True(_hashService.VerifyAdminPassword("secret", admin, out var needsRehash));
        Xunit.Assert.False(needsRehash);
        Xunit.Assert.False(_hashService.VerifyAdminPassword("wrong", admin, out _));
    }

    [Fact]
    public void VerifyAdminPassword_AcceptsLegacySha256_AndFlagsRehash()
    {
        var admin = new AdminModel { Login = "a", Password = _hashService.HashPassword("secret") };

        Xunit.Assert.True(_hashService.VerifyAdminPassword("secret", admin, out var needsRehash));
        Xunit.Assert.True(needsRehash);
        Xunit.Assert.False(_hashService.VerifyAdminPassword("wrong", admin, out var wrongRehash));
        Xunit.Assert.False(wrongRehash);
    }
}
