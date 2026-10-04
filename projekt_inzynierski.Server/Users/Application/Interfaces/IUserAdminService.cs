using projekt_inzynierski.Server.Users.Application.DTOs;
using projekt_inzynierski.Server.Users.Domain.Models;

namespace projekt_inzynierski.Server.Users.Application.Interfaces
{
    public interface IUserAdminService
    {
        Task<int> GetUserCount();
        Task<PagedResult<User>> GetUsersPagedAsync(int page, int pageSize);
         Task BanUser(int userId);
         Task UnbanUser(int userId);
    }
}
