using projekt_inzynierski.Server.PremiumStore.Domain.Models;

namespace projekt_inzynierski.Server.PremiumStore.Application.Interfaces
{
    public interface IAvatarRepositoryInterface
    {
        Task<bool> UnlockAvatar(string userId, int avatarId);
        Task<bool> SelectAvatar(string userId, int avatarId);
        Task<Avatar> GetAvatarByRewardId(int id);
        Task AddUserAvatar(UserAvatar us);
        Task<List<Avatar>> GetUserAvatars(string userId);

    }
}