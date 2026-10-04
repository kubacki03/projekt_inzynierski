

using projekt_inzynierski.Server.Achievments.Application.DTOs;
using projekt_inzynierski.Server.Achievments.Domain.Models;

namespace projekt_inzynierski.Server.Achievments.Application.Interfaces
{
    public interface IUserAchievment
    {
        List<AchievmentDto> GetUserAchievments(string id);

        Task<List<string>> GetMostActiveUsersId();
        Task<float> GetCompletedAchievmentsPercentage(string userId);
        Task SaveUserAchievement(UserAchievement userAchievement);
    }
}
