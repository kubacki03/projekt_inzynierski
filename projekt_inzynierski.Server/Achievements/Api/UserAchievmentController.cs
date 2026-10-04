using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using projekt_inzynierski.Server.Achievments.Application.DTOs;
using projekt_inzynierski.Server.Achievments.Application.Interfaces;
using projekt_inzynierski.Server.Achievments.Domain.Models;
using projekt_inzynierski.Server.Achievments.Infrastructures.Services;

namespace projekt_inzynierski.Server.Achievments.Api
{

    [ApiController]
    [Route("[controller]")]
    public class UserAchievmentController : Controller
    {
        private readonly IUserAchievment _service;

        public UserAchievmentController(IUserAchievment service)
        {
            _service = service;
        }

        [HttpGet("GetUserAchievments")]
        [Authorize]
        public async Task<IActionResult> GetUserAchievments()
        {

            var user = User.Identity.Name;
            var list = _service.GetUserAchievments(user);
        
            return Ok(list);
        }


        [HttpGet("GetMostActiveUsers")]
        public async Task<IActionResult> GetMostActiveUsers()
        {
            var result = await _service.GetMostActiveUsersId();
            return Ok(result); 
        }

        [HttpGet("GetAchievementPercentage")]
        [Authorize]
        public async Task<IActionResult> GetAchievementPercentage()
        {
            var user = User.Identity.Name;
            var result = await _service.GetCompletedAchievmentsPercentage(user);
            return Ok(result);
        }
        [HttpGet("FavouriteTopics")]
        [Authorize]
        public async Task<IActionResult> FavouriteTopics()
        {
            var user = User.Identity.Name;
            var result = _service.GetUserAchievments(user)
          .GroupBy(x => x.Category)
          .OrderByDescending(g => g.Count())
          .Select(g => g.Key)
          .FirstOrDefault();

            return Ok(result);
        }

    }
}
