using Microsoft.EntityFrameworkCore;
using projekt_inzynierski.Server.Achievments.Domain.Models;
using projekt_inzynierski.Server.Achievments.Domain.Repositories;

namespace projekt_inzynierski.Server.Achievments.Infrastructures.Persistance
{
    public class AchievmentRepository : IAchievmentRepository
    {
        private readonly AchievementsDbContext _context;

        public AchievmentRepository(AchievementsDbContext context)
        {
            _context = context;
        }

        public List<string> GetMostActiveUsersId()
        {
            return _context.UserAchievements
                .GroupBy(ua => ua.UserId)
                .OrderByDescending(g => g.Count())
                .Take(5)
                .Select(g => (g.Key.ToString()))
                .ToList();
        }

        public List<UserAchievement> GetUserAchievments(string id)
        {
            return  _context.UserAchievements.Include(z=>z.Achievement).Where(a=>a.UserId == Guid.Parse(id)).ToList();
        }

        public async Task SaveUserAchievement(UserAchievement userAchievement)
        {
            await _context.UserAchievements.AddAsync(userAchievement);
            await _context.SaveChangesAsync();
        }



        public async Task<float> GetCompletedAchievementPercentage(string userId)
        {
            float a = _context.UserAchievements.Where(x => x.UserId == Guid.Parse(userId)).Count();
            float b =await _context.Achievements.CountAsync();
            return a / b;
        }
    }
}
