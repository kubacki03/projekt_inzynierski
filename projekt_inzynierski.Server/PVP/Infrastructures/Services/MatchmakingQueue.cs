using projekt_inzynierski.Server.PVP.Application.Interfaces;
using projekt_inzynierski.Server.PVP.Domain.Models;

namespace projekt_inzynierski.Server.PVP.Infrastructures.Services
{
    public class MatchmakingQueue : IMatchmakingQueue
    {
        private record QueueEntry(string UserId, string ConnectionId);

        private static readonly List<PvpGame> Games = new()
        {
            new PvpGame(1, "javascript", "beginner"),
            new PvpGame(2, "javascript", "advanced"),
            new PvpGame(3, "python", "beginner"),
            new PvpGame(4, "python", "advanced"),
            new PvpGame(5, "csharp", "beginner"),
            new PvpGame(6, "csharp", "advanced"),
        };

        private readonly Dictionary<int, List<QueueEntry>> _queue = new();
        private readonly Dictionary<string, MatchResult> _sessions = new();
        private readonly object _lock = new();

        public List<PvpGameDto> GetGames()
        {
            lock (_lock)
            {
                return Games
                    .Select(g => new PvpGameDto(g.Id, g.Technology, g.Level,
                        _queue.TryGetValue(g.Id, out var q) ? q.Count : 0))
                    .ToList();
            }
        }

        public bool GameExists(int gameId) => Games.Any(g => g.Id == gameId);

        public MatchResult? Enqueue(int gameId, string userId, string connectionId)
        {
            lock (_lock)
            {
                RemoveUser(userId);

                if (!_queue.TryGetValue(gameId, out var queue))
                {
                    queue = new List<QueueEntry>();
                    _queue[gameId] = queue;
                }

                var opponent = queue.FirstOrDefault(e => e.UserId != userId);
                if (opponent == null)
                {
                    queue.Add(new QueueEntry(userId, connectionId));
                    return null;
                }

                queue.Remove(opponent);
                var match = new MatchResult
                {
                    SessionId = Guid.NewGuid().ToString(),
                    GameId = gameId,
                    FirstUserId = opponent.UserId,
                    SecondUserId = userId,
                    FirstConnectionId = opponent.ConnectionId,
                    SecondConnectionId = connectionId
                };
                _sessions[match.SessionId] = match;
                return match;
            }
        }

        public void RemoveConnection(string connectionId)
        {
            lock (_lock)
            {
                foreach (var queue in _queue.Values)
                {
                    queue.RemoveAll(e => e.ConnectionId == connectionId);
                }
            }
        }

        public MatchResult? GetSession(string sessionId)
        {
            lock (_lock)
            {
                return _sessions.TryGetValue(sessionId, out var session) ? session : null;
            }
        }

        private void RemoveUser(string userId)
        {
            foreach (var queue in _queue.Values)
            {
                queue.RemoveAll(e => e.UserId == userId);
            }
        }
    }
}
