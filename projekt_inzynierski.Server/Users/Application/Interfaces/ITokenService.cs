using projekt_inzynierski.Server.Users.Domain.Models;

namespace projekt_inzynierski.Server.Users.Application.Interfaces
{
    public interface ITokenService
    {
        string CreateUserToken(User user);
        string CreateAdminToken(int adminId);
    }
}
