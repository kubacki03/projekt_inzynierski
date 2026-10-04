using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using projekt_inzynierski.Server.PremiumStore.Application.Interfaces;

namespace projekt_inzynierski.Server.PremiumStore.Api
{
    [ApiController]
    [Route("[controller]")]
    public class StoreController : Controller
    {

        private readonly IStoreService _service;
        public StoreController(IStoreService service)
        {
            _service = service;
        }

        [HttpGet("Get")]
        [Authorize]
        public async Task<IActionResult> GetAllRewards()
        {
            var userId = User.Identity.Name;
            return Ok(await _service.GetAllRewardsAsync(userId));
        }

        [HttpPost("BuyReward")]
        [Authorize]
        public async Task<IActionResult> BuyReward(int rewardId)
        {
            var userId = User.Identity.Name;

            var buy = await _service.GetReward(rewardId, userId);
            if (buy)
            {
                return Ok();
            }
            else
            {
                return Conflict();
            }
        }

    }
}