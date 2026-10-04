using System.Text.Json.Serialization;

namespace projekt_inzynierski.Server.PVP.Domain.Models
{
    public class MatchResult
    {
        public required string SessionId { get; init; }
        public required int GameId { get; init; }
        public required string FirstUserId { get; init; }
        public required string SecondUserId { get; init; }
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

        [JsonIgnore]
        public string FirstConnectionId { get; init; } = "";
        [JsonIgnore]
        public string SecondConnectionId { get; init; } = "";

        public bool HasPlayer(string userId) => FirstUserId == userId || SecondUserId == userId;
    }
}
