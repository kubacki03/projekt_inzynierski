using projekt_inzynierski.Server.Achievments.Application.DTOs;
using projekt_inzynierski.Server.Achievments.Application.Interfaces;
using projekt_inzynierski.Server.Achievments.Domain.Models;
using projekt_inzynierski.Server.Achievments.Domain.Repositories;
using projekt_inzynierski.Server.Achievments.Infrastructures.Persistance;
using projekt_inzynierski.Server.Users.Domain.Repositories;

namespace projekt_inzynierski.Server.Achievments.Infrastructures.Services
{
    public class UserAchievmentService : IUserAchievment
    {
        private readonly IAchievmentRepository _repository;
        private readonly IUserRepository _userRepository;
        public UserAchievmentService(IAchievmentRepository repository, IUserRepository user )
        {
            _repository = repository;
            _userRepository = user;
        }

        public async Task<List<string>> GetMostActiveUsersId()
        {
            var list = _repository.GetMostActiveUsersId();
            var list2 =await _userRepository.GetUsersByPublicId( list );
            return list2.Select(s=>s.Nickname).ToList();
        }

        public List<AchievmentDto> GetUserAchievments(string id)
        {
            var list = _repository.GetUserAchievments(id);

            List<AchievmentDto> list2 = new List<AchievmentDto>();
            list.ForEach(a => list2.Add(new AchievmentDto { Name = a.Achievement.Name, Description = a.Achievement.Description, Date = a.EarnedAtUtc, Category = a.Achievement.SkillCategory, Level = a.Achievement.BadgeType, Progress = 1 }));

            return list2;
           
        }

        public async Task SaveUserAchievement(UserAchievement userAchievement)
        {
         await   _repository.SaveUserAchievement(userAchievement);
        }

        public async Task<float> GetCompletedAchievmentsPercentage(string userId)
        {
            return await _repository.GetCompletedAchievementPercentage(userId);
        }
    }
}
