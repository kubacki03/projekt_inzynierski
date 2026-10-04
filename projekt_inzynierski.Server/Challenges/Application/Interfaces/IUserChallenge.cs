using projekt_inzynierski.Server.Achievments.Domain.Models;
using projekt_inzynierski.Server.Challenges.Application.DTOs;
using projekt_inzynierski.Server.Challenges.Domain.Models;
using projekt_inzynierski.Server.Challenges.Infrastructures.Persistance;

namespace projekt_inzynierski.Server.Challenges.Application.Interfaces
{
    public interface IUserChallenge
    {
        Task<List<UserChallengeDto>> GetUserChallenges(string userId);
        Task AddUserChallange(string userId);
        Task<Challenge> GetChallengeAsync(int id);
        Task<bool> VerifyChallenge(string userId, int id, string task);

         Task<List<Badge>> GetUserBadges(string userId);


          Task AddUserBadge(string userId, int badgeId);
        
    }
}