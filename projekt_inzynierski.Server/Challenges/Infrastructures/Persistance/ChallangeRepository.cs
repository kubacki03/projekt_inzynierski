using Microsoft.EntityFrameworkCore;
using projekt_inzynierski.Server.Achievments.Domain.Models;
using projekt_inzynierski.Server.Challenges.Domain.Repositories;

namespace projekt_inzynierski.Server.Challenges.Infrastructures.Persistance
{
    public class ChallangeRepository : IChallengeRepository
    {
        private readonly ChallengeDbContext _context;
        public ChallangeRepository(ChallengeDbContext achievementsDbContext)
        {
            _context = achievementsDbContext;
        }


        public async Task<Challenge> GetChallengeAsync(int id)
        {
            return await _context.Challenges.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateChallengeStatus(int id, string userId)
        {
            var x = await _context.UserChallenges.FirstOrDefaultAsync(x => x.Id == id);
            if (x == null)
            {
                var us = new UserChallenge { UserId = userId, ChallengeId = id, Completed = true };
                _context.UserChallenges.Add(us);
                await _context.SaveChangesAsync();
                return;
            }
            x.Completed = true;
            await _context.SaveChangesAsync();
        }
    }
}