using projekt_inzynierski.Server.Achievments.Domain.Models;
using projekt_inzynierski.Server.AIHelper.Application.Interfaces;
using projekt_inzynierski.Server.Challenges.Application.DTOs;
using projekt_inzynierski.Server.Challenges.Application.Interfaces;
using projekt_inzynierski.Server.Challenges.Domain.Models;
using projekt_inzynierski.Server.Challenges.Domain.Repositories;
using projekt_inzynierski.Server.Challenges.Infrastructures.Persistance;
using projekt_inzynierski.Server.Content.Application.DTOs;
using projekt_inzynierski.Server.Users.Application.Interfaces;

namespace projekt_inzynierski.Server.Challenges.Infrastructures.Services
{
    public class UserChallengeService : IUserChallenge
    {
        private readonly IUserChallangeRepository _userChallangeRepository;
        private readonly IChallengeRepository _challangeRepository;
        private readonly ICodeAnalyzer _codeAnalyzer;
        private readonly IUserProgress _authService;
        public UserChallengeService(IUserChallangeRepository userChallangeRepository, IChallengeRepository challangeRepository, ICodeAnalyzer codeAnalyzer, IUserProgress authService)
        {
            _authService = authService;
            _userChallangeRepository = userChallangeRepository;
            _challangeRepository = challangeRepository;
            _codeAnalyzer = codeAnalyzer;
        }

        public async Task<List<UserChallengeDto>> GetUserChallenges(string userId)
        {
            var list = _userChallangeRepository.GetUserChallenges(userId);
            var weeklyList = await _userChallangeRepository.GetUserWeeklyChallengesDto(userId);

            List<UserChallengeDto> userChallengeDtos = new List<UserChallengeDto>();

            var weeklyIds = weeklyList.Select(w => w.Id).ToHashSet();

            foreach (var u in list.Where(x => !weeklyIds.Contains(x.ChallengeId)))
            {
                userChallengeDtos.Add(new UserChallengeDto
                {
                    Name = u.Challenge.Name,
                    Description = u.Challenge.Description,
                    Points = u.Challenge.Points,
                    RewardDescription = u.Challenge.RewardDescription,
                    Type = u.Challenge.Type,
                    isDone = u.Completed,
                    isWeekly = false,
                    Id = u.ChallengeId
                });
            }

            userChallengeDtos.AddRange(weeklyList);
            return userChallengeDtos;
        }



        public async Task AddUserChallange(string userId)
        {
            await _userChallangeRepository.AddUserChallange(userId);
        }

        public async Task<Challenge> GetChallengeAsync(int id)
        {
            return await _challangeRepository.GetChallengeAsync(id);
        }


        public async Task<bool> VerifyChallenge(string userId, int id, string task)
        {
            var achievement = await GetChallengeAsync(id);
            
            var x = await _codeAnalyzer.AnalyzeCode(task, achievement.Description);
            // CodeReviewResult x = new CodeReviewResult { IsDoneGood = true,Review="as" };
            if (x.IsDoneGood)
            {
                await _authService.IncreaseUserGoldenPoints(userId, achievement.Points);
                await _challangeRepository.UpdateChallengeStatus(achievement.Id, userId);
                await AddUserBadge(userId, achievement.BadgeId);
            }

            return x.IsDoneGood;
        }

        public async Task<List<Badge>> GetUserBadges(string userId)
        {
            return await _userChallangeRepository.GetUserBadges(userId);
        }

        public async Task AddUserBadge(string userId, int badgeId)
        {
            await _userChallangeRepository.AddUserBadge(userId, badgeId);
        }

    }
}