using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using projekt_inzynierski.Server.PVP.Application.DTOs;
using projekt_inzynierski.Server.PVP.Application.Interfaces;
using projekt_inzynierski.Server.PVP.Domain.Models;

namespace projekt_inzynierski.Server.PVP.Infrastructures.Hubs
{
    [Authorize]
    public class GameHub : Hub
    {
        private readonly IMatchmakingQueue _matchmakingQueue;
        private readonly IPvpMatchManager _matchManager;

        public GameHub(IMatchmakingQueue matchmakingQueue, IPvpMatchManager matchManager)
        {
            _matchmakingQueue = matchmakingQueue;
            _matchManager = matchManager;
        }

        public async Task JoinQueue(int gameId)
        {
            var userId = GetUserId();
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

        public async Task<PvpStateDto> JoinSession(string sessionId)
        {
            var userId = GetUserId();
            if (!_matchManager.Join(sessionId, userId))
            {
                throw new HubException("Game not found");
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, PvpMatch.GroupName(sessionId));

            var state = _matchManager.GetState(sessionId, userId);
            if (state == null)
            {
                throw new HubException("Game not found");
            }
            return state;
        }

        public Task SubmitAnswer(string sessionId, int questionIndex, int answerIndex)
        {
            _matchManager.SubmitAnswer(sessionId, GetUserId(), questionIndex, answerIndex);
            return Task.CompletedTask;
        }

        public Task ReportFocusLost(string sessionId)
        {
            return _matchManager.ReportFocusLostAsync(sessionId, GetUserId());
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _matchmakingQueue.RemoveConnection(Context.ConnectionId);
            await Clients.Others.SendAsync("QueueUpdated", _matchmakingQueue.GetGames());
            await base.OnDisconnectedAsync(exception);
        }

        private string GetUserId()
        {
            var userId = Context.User?.Identity?.Name;
            if (string.IsNullOrEmpty(userId))
            {
                throw new HubException("Unauthorized");
            }
            return userId;
        }
    }
}
