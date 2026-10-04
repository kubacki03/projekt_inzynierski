using projekt_inzynierski.Server.PremiumStore.Application.Interfaces;
using projekt_inzynierski.Server.PremiumStore.Application.Repositories;
using projekt_inzynierski.Server.PremiumStore.Domain.Models;
using projekt_inzynierski.Server.Users.Application.Interfaces;
using projekt_inzynierski.Server.Users.Domain.Repositories;

namespace projekt_inzynierski.Server.PremiumStore.Infrastructures.Services
{
    public class StoreService : IStoreService
    {

        private readonly IRewardRepository _rewardRepository;
        private IAuthService _authService;
        private IUserRepository _userRepository;
        private readonly IAvatarRepositoryInterface _avatarRepository;
        private readonly IUserProgress _userProgress;

        public StoreService(IRewardRepository rewardRepository, IAuthService authService, IUserRepository userRepository, IAvatarRepositoryInterface avatarRepositoryInterface, IUserProgress userProgress)
        {
            _avatarRepository = avatarRepositoryInterface;
            _userRepository = userRepository;
            _authService = authService;
            _rewardRepository = rewardRepository;
            _userProgress = userProgress;
        }

        public async Task<List<Reward>> GetAllRewardsAsync(string userId)
        {
            return await _rewardRepository.GetAllRewardsAsync(userId);
        }


        public async Task<bool> GetReward(int rewardId, string userId)
        {
            var reward = await _rewardRepository.GetRewardAsync(rewardId);
            if (reward == null || reward.Cost < 0) return false;

            // Atomic check-and-deduct; fails when the balance is insufficient.
            if (!await _userProgress.TrySpendGoldenPoints(userId, reward.Cost)) return false;

            try
            {
                return await GrantReward(reward, rewardId, userId);
            }
            catch
            {
                // Granting failed after the payment went through: give the points back.
                await _userProgress.IncreaseUserGoldenPoints(userId, reward.Cost);
                throw;
            }
        }

        private async Task<bool> GrantReward(Reward reward, int rewardId, string userId)
        {
            UserReward userReward = new UserReward { RewardId = rewardId, UserPublicId = Guid.ParseExact(userId, "D") };
            await _rewardRepository.AddUserReward(userReward);

            switch (reward.Type)
            {
                case "Awatar":
                    Avatar av = await _avatarRepository.GetAvatarByRewardId(rewardId);
                    UserAvatar us = new UserAvatar { AvatarId = av.Id, IsSelected = false, UnlockedAt = DateTime.UtcNow, UserId = userId };
                    await _avatarRepository.AddUserAvatar(us);
                    await _userRepository.SetAvatar(av, userId);
                    break;
                case "Premium1":
                   await _userRepository.ExtendPremiumAccountDate(userId, 1);
                    break;
                case "Premium7":
                    await _userRepository.ExtendPremiumAccountDate(userId, 7);
                    break;
                case "Premium30":
                    await _userRepository.ExtendPremiumAccountDate(userId, 30);
                    break;
            }

            return true;
        }



    }
}