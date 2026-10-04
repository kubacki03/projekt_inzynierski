using projekt_inzynierski.Server.Achievments.Domain.Models;

namespace projekt_inzynierski.Server.Achievments.Domain.Repositories
{
    public interface IAchievmentRepository
    {
        List<UserAchievement> GetUserAchievments(string id);

        List<string> GetMostActiveUsersId();
        Task SaveUserAchievement(UserAchievement userAchievement);
        Task<float> GetCompletedAchievementPercentage(string userId);
    }
}
