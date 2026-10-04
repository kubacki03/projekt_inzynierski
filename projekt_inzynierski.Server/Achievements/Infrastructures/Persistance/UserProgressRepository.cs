using projekt_inzynierski.Server.Achievments.Application.Repositories;
using projekt_inzynierski.Server.Achievments.Domain.Models;

namespace projekt_inzynierski.Server.Achievments.Infrastructures.Persistance
{
    public class UserProgressRepository : IUserProgressRepository
    {
        private readonly AchievementsDbContext _context;

        public UserProgressRepository(AchievementsDbContext achievementsDbContext)
        {
            _context = achievementsDbContext;
        }

        public async Task AddUserProgress(UserProgress userProgress)
        {
            await _context.AddAsync(userProgress);
        }

        
    }
}
