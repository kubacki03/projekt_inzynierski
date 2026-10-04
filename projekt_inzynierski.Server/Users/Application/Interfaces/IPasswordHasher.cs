using projekt_inzynierski.Server.Users.Domain.Models;
using AdminModel = projekt_inzynierski.Server.Users.Domain.Models.Admin;

namespace projekt_inzynierski.Server.Users.Application.Interfaces
{
    public interface IPasswordHasher
    {
        string Hash(string password, User user);
        string HashPassword(string password);
        bool VerifyPassword(string password, User user);
        string HashAdminPassword(string password, AdminModel admin);
        bool VerifyAdminPassword(string password, AdminModel admin, out bool needsRehash);
    }
}
