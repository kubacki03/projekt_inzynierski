using Microsoft.EntityFrameworkCore;
using projekt_inzynierski.Server.Challenges.Application.DTOs;
using projekt_inzynierski.Server.Challenges.Domain.Models;
using projekt_inzynierski.Server.Challenges.Domain.Repositories;


namespace projekt_inzynierski.Server.Challenges.Infrastructures.Persistance
{
    public class UserChallangeRepository : IUserChallangeRepository
    {
        private readonly ChallengeDbContext _context;

        public UserChallangeRepository(ChallengeDbContext context)
        {
            _context = context;
        }

        public async Task AddUserChallange(string userId)
        {
            await _context.AddAsync(new UserChallenge { ChallengeId = 19, Completed = false, UserId = userId });
            await _context.SaveChangesAsync();
        }

        public async Task AddUserBadge(string userId, int badgeId)
        {
            var us = new UserBadge { BadgeId = badgeId, UserId = userId };
            await _context.UserBadges.AddAsync(us);
            await _context.SaveChangesAsync();
        }


        public async Task<List<Badge>> GetUserBadges(string userId)
        {
            return await _context.UserBadges.Include(x => x.Badge).Where(x => x.UserId == userId).Select(x => x.Badge).ToListAsync();
        }

        public List<UserChallenge> GetUserChallenges(string userId)
        {
            return _context.UserChallenges.Include(a => a.Challenge).Where(a => a.UserId == userId).ToList();

        }

        public async Task<List<UserChallengeDto>> GetUserWeeklyChallengesDto(string userId)
        {
            var weekly = _context.WeeklyChallenges.Include(x => x.Challenge).Select(a => new UserChallengeDto { Description = a.Challenge.Description, Name = a.Challenge.Name, Points = a.Challenge.Points, RewardDescription = a.Challenge.RewardDescription, Type = a.Challenge.Type, isWeekly = true, Id = a.ChallengeId }).ToList();

            foreach (var z in weekly)
            {
                var y = await _context.UserChallenges.FirstOrDefaultAsync(x => x.ChallengeId == z.Id && x.UserId == userId);
                if (y != null && y.Completed)
                {
                    z.isDone = true;
                }
            }
            return weekly;
        }
    }
}