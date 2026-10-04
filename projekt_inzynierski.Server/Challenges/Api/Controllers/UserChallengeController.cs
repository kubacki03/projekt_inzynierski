using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using projekt_inzynierski.Server.AIHelper.Application.Interfaces;
using projekt_inzynierski.Server.Challenges.Application.DTOs;
using projekt_inzynierski.Server.Challenges.Application.Interfaces;


namespace projekt_inzynierski.Server.Challenges.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserChallengeController : ControllerBase
    {

        private readonly IUserChallenge _userChallengeService;
        private readonly ICodeAnalyzer _codeAnalyzer;
        public UserChallengeController(IUserChallenge userChallengeService, ICodeAnalyzer codeAnalyzer)
        {
            _codeAnalyzer = codeAnalyzer;
            _userChallengeService = userChallengeService;
        }

        [HttpGet("Get")]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var userId = User.Identity?.Name;

            if (userId == null)
            {
                return NotFound();
            }
            var x = await _userChallengeService.GetUserChallenges(userId);
            return Ok(x);
        }

        [HttpPost("Submit")]
        [Authorize]
        public async Task<IActionResult> VerifyChallenge(VerifyRequestDto requestDto)
        {
            var userId = User.Identity?.Name;

            if (userId == null)
            {
                return NotFound();
            }

            var result = await _userChallengeService.VerifyChallenge(userId, requestDto.AchievementId, requestDto.Task);
            return Ok(result);
        }

        [HttpGet("GetBadges")]
        [Authorize]
        public async Task<IActionResult> GetUserBadges()
        {
            var userId = User.Identity?.Name;

            if (userId == null)
            {
                return NotFound();
            }

            var result = await _userChallengeService.GetUserBadges(userId);
            return Ok(result);
        }



    }
}