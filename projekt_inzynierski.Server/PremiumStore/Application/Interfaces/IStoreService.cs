using projekt_inzynierski.Server.PremiumStore.Domain.Models;

namespace projekt_inzynierski.Server.PremiumStore.Application.Interfaces
{
    public interface IStoreService
    {
        Task<List<Reward>> GetAllRewardsAsync(string userId);
        Task<bool> GetReward(int rewardId, string userId);
    }
}