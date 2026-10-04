using projekt_inzynierski.Server.PremiumStore.Domain.Models;

namespace projekt_inzynierski.Server.PremiumStore.Application.Repositories
{
    public interface IRewardRepository
    {
        Task<List<Reward>> GetAllRewardsAsync(string userId);
        Task<Reward> GetRewardAsync(int id);
        Task AddUserReward(UserReward ur);
        Task<Avatar> GetAvatarById(int id);
    }
}