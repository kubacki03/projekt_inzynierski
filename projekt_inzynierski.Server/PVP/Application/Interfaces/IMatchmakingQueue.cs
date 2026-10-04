using projekt_inzynierski.Server.PVP.Domain.Models;

namespace projekt_inzynierski.Server.PVP.Application.Interfaces
{
    public interface IMatchmakingQueue
    {
        List<PvpGameDto> GetGames();
        bool GameExists(int gameId);
        MatchResult? Enqueue(int gameId, string userId, string connectionId);
        void RemoveConnection(string connectionId);
        MatchResult? GetSession(string sessionId);
    }
}
