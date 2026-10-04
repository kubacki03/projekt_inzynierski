using projekt_inzynierski.Server.Achievments.Domain.Models;
using projekt_inzynierski.Server.Challenges.Infrastructures.Persistance;

namespace projekt_inzynierski.Server.Challenges.Domain.Repositories
{
    public interface IChallengeRepository
    {

        Task<Challenge> GetChallengeAsync(int id);
        Task UpdateChallengeStatus(int id, string userId);
    }
}