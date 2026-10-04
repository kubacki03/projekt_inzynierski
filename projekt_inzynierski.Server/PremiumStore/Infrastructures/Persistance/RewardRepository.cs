using iText.Commons.Actions.Contexts;
using Microsoft.EntityFrameworkCore;
using projekt_inzynierski.Server.PremiumStore.Application.Repositories;
using projekt_inzynierski.Server.PremiumStore.Domain.Models;

namespace projekt_inzynierski.Server.PremiumStore.Infrastructures.Persistance
{
    public class RewardRepository : IRewardRepository
    {
        private readonly StoreDbContext _dbContext;
        public RewardRepository(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Reward>> GetAllRewardsAsync(string userId)
        {
            var all = await _dbContext.Rewards.AsNoTracking().ToListAsync();
            var userReward =await _dbContext.UserRewards.AsNoTracking().Include(x => x.Reward).Where(x => x.UserPublicId == Guid.Parse(userId)).Select(x=>x.Reward).ToListAsync();
            
            foreach(var x in userReward)
            {
                try
                {
                    if (x.Type != "Premium1" || x.Type != "Premium7" || x.Type != "Premium30")
                    {
                        all.Remove(x);
                    }
                }catch(Exception ex)
                {
                    Console.WriteLine(ex);
                } 
            } 
            return all;
        }

        public async Task<Reward> GetRewardAsync(int id)
        {
            return await _dbContext.Rewards.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddUserReward(UserReward ur)
        {
            await _dbContext.UserRewards.AddAsync(ur);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Avatar> GetAvatarById(int id)
        {
            return await _dbContext.Avatars.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}