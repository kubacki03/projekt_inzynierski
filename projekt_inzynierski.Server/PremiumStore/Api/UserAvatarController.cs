using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using projekt_inzynierski.Server.PremiumStore.Application.Interfaces;
using projekt_inzynierski.Server.Users.Domain.Repositories;

namespace projekt_inzynierski.Server.PremiumStore.Api
{
    [ApiController]
    [Route("[controller]")]
    public class UserAvatarController : Controller
    {

        private readonly IUserAvatar _service;
        
        public UserAvatarController(IUserAvatar service)
        {
            _service = service;
        }   

        [HttpGet("GetUserAvatars")]
        [Authorize]
        public async Task<IActionResult> GetUserAvatars()
        {
            var userId = User.Identity.Name;
            return Ok(await _service.GetUserAvatar(userId));
        }

        [HttpPut("ChangeAvatar")]
        [Authorize]
        public async Task<IActionResult> ChangeAvatar(int id)
        {
            var userId = User.Identity.Name;
            await _service.ChangeAvatar(id, userId);
            return  Ok();
        }
    }
}
