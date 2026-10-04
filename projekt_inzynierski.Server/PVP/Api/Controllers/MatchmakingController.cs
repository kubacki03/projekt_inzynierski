using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using projekt_inzynierski.Server.PVP.Application.Interfaces;
using projekt_inzynierski.Server.Users.Domain.Repositories;

namespace projekt_inzynierski.Server.PVP.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class MatchmakingController : ControllerBase
    {
        private readonly IMatchmakingQueue _matchmakingQueue;
        private readonly IUserRepository _userRepository;

        public MatchmakingController(IMatchmakingQueue matchmakingQueue, IUserRepository userRepository)
        {
            _matchmakingQueue = matchmakingQueue;
            _userRepository = userRepository;
        }

        [HttpGet("games")]
        public IActionResult GetGames()
        {
            return Ok(_matchmakingQueue.GetGames());
        }

        [HttpGet("session/{sessionId}")]
        public async Task<IActionResult> GetSession(string sessionId)
        {
            var userId = User.Identity?.Name;
            if (userId == null)
            {
                return Unauthorized();
            }

            var session = _matchmakingQueue.GetSession(sessionId);
            if (session == null)
            {
                return NotFound();
            }
            if (!session.HasPlayer(userId))
            {
                return Forbid();
            }

            var game = _matchmakingQueue.GetGames().First(g => g.Id == session.GameId);
            var opponentId = session.FirstUserId == userId ? session.SecondUserId : session.FirstUserId;
            var opponent = await _userRepository.GetUserByPublicIdAsync(opponentId);

            return Ok(new
            {
                session.SessionId,
                game.Technology,
                game.Level,
                OpponentId = opponentId,
                OpponentNickname = opponent?.Nickname ?? "Przeciwnik",
                session.CreatedAt
            });
        }
    }
}
