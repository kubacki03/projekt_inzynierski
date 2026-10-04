using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using projekt_inzynierski.Server.PVP.Application.Interfaces;

namespace projekt_inzynierski.Server.PVP.Infrastructures.Hubs
{
    [Authorize]
    public class GameHub : Hub
    {
        private readonly IMatchmakingQueue _matchmakingQueue;

        public GameHub(IMatchmakingQueue matchmakingQueue)
        {
            _matchmakingQueue = matchmakingQueue;
        }

        public async Task JoinQueue(int gameId)
        {
            var userId = Context.User?.Identity?.Name;
            if (string.IsNullOrEmpty(userId))
            {
                throw new HubException("Unauthorized");
            }
            if (!_matchmakingQueue.GameExists(gameId))
            {
                throw new HubException("Game not found");
            }

            var match = _matchmakingQueue.Enqueue(gameId, userId, Context.ConnectionId);
            if (match == null)
            {
                await Clients.Caller.SendAsync("WaitingForOpponent", gameId);
            }
            else
            {
                await Clients.Clients(match.FirstConnectionId, match.SecondConnectionId)
                    .SendAsync("MatchFound", match.SessionId);
            }
            await Clients.All.SendAsync("QueueUpdated", _matchmakingQueue.GetGames());
        }

        public async Task LeaveQueue()
        {
            _matchmakingQueue.RemoveConnection(Context.ConnectionId);
            await Clients.All.SendAsync("QueueUpdated", _matchmakingQueue.GetGames());
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _matchmakingQueue.RemoveConnection(Context.ConnectionId);
            await Clients.Others.SendAsync("QueueUpdated", _matchmakingQueue.GetGames());
            await base.OnDisconnectedAsync(exception);
        }
    }
}
