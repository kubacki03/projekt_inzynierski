using Microsoft.EntityFrameworkCore;
using projekt_inzynierski.Server.Users.Application.DTOs;
using projekt_inzynierski.Server.Users.Domain.Models;

namespace projekt_inzynierski.Server.Users.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> LoginAdmin(string login, string password);
        Task<Guid> RegisterAsync( UserRegisterDto dto);
        Task<string> GenerateJWT(User user);
        Task<User> GetUserByPublicIdAsync(string publicId);
      
        Task<string> LoginAsync(string email, string password);
    }
}
