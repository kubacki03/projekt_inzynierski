using projekt_inzynierski.Server.PremiumStore.Domain.Models;
using projekt_inzynierski.Server.Users.Application.DTOs;
using projekt_inzynierski.Server.Users.Domain.Models;
namespace projekt_inzynierski.Server.Users.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<projekt_inzynierski.Server.Users.Domain.Models.Admin> GetAdminByLogin(string login);
        Task UpdateAdminPasswordHash(int adminId, string passwordHash);
        Task AddAsync(projekt_inzynierski.Server.Users.Domain.Models.User user);
        Task<PagedResult<User>> GetUsersPagedAsync(int page, int pageSize);
        Task<User> GetUserByEmail(string email);
        Task<List<UserProgressAvatarDto>> GetMostActiveUsers();
        Task<long> GetUserProgress(string publicId);
        Task<User> GetUserByPublicIdAsync(string publicId);
        Task<List<User>> GetUsersByPublicId(List<string> listId);
        Task IncreaseUserProgress(string publicId, int points);
        Task<long> IncreaseUserGoldenPoints(string userId, int p);
        Task<long> DecreaseGoldenPoints(string userId, int p);
        Task<long?> TryDecreaseGoldenPoints(string userId, int p);
        Task SetAvatar(Avatar us, string userId);
        Task SetNewUserAvatar(int id, string userId);
        Task<DateTime> ExtendPremiumAccountDate( string userId, int days);
    }
}