
using projekt_inzynierski.Server.Challenges.Application.DTOs;
using projekt_inzynierski.Server.Challenges.Domain.Models;
using projekt_inzynierski.Server.Challenges.Infrastructures.Persistance;

namespace projekt_inzynierski.Server.Challenges.Domain.Repositories
{
    public interface IUserChallangeRepository
    {
        List<UserChallenge> GetUserChallenges(string userId);

        Task<List<UserChallengeDto>> GetUserWeeklyChallengesDto(string userId);
        Task AddUserBadge(string userId, int badgeId);
        Task<List<Badge>> GetUserBadges(string userId);
        Task AddUserChallange(string userId);
    }
}