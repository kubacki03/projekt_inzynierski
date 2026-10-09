namespace projekt_inzynierski.Server.PVP.Domain.Models
{
    public enum PvpMatchStatus
    {
        WaitingForPlayers,
        InProgress,
        Finished
    }

    public class PvpAnswerOutcome
    {
        public required int QuestionIndex { get; init; }
        public required string UserId { get; init; }
        public required int SelectedIndex { get; init; }
        public required int CorrectIndex { get; init; }
        public required bool IsCorrect { get; init; }
        public required int ElapsedMs { get; init; }
    }

    public class PvpMatch
    {
        public const int QuestionCount = 10;
        public const int SecondsPerQuestion = 20;
        public const int MaxFocusLosses = 3;
        public const int RewardGoldenPoints = 20;

        public PvpMatch(string sessionId, string firstUserId, string secondUserId, IReadOnlyList<PvpQuestion> questions)
        {
            SessionId = sessionId;
            Players = new[] { firstUserId, secondUserId };
            Questions = questions;
            foreach (var player in Players)
            {
                Scores[player] = 0;
                ElapsedTotals[player] = 0;
                FocusLosses[player] = 0;
            }
        }

        public object Sync { get; } = new();
        public string SessionId { get; }
        public string[] Players { get; }
        public IReadOnlyList<PvpQuestion> Questions { get; }
        public PvpMatchStatus Status { get; set; } = PvpMatchStatus.WaitingForPlayers;
        public HashSet<string> JoinedPlayers { get; } = new();
        public Dictionary<string, int> Scores { get; } = new();
        public Dictionary<string, int> ElapsedTotals { get; } = new();
        public Dictionary<string, int> FocusLosses { get; } = new();
        public int CurrentIndex { get; set; } = -1;
        public DateTime QuestionDeadline { get; set; }
        public DateTime QuestionStartedAt { get; set; }
        public PvpAnswerOutcome? CurrentOutcome { get; set; }
        public TaskCompletionSource? AnswerSignal { get; set; }
        public CancellationTokenSource Cts { get; } = new();
        public string? WinnerId { get; set; }
        public string? FinishReason { get; set; }
        public int CoinsAwarded { get; set; }

        public static string GroupName(string sessionId) => $"pvp-{sessionId}";

        public bool HasPlayer(string userId) => Players.Contains(userId);

        public string OpponentOf(string userId) => Players[0] == userId ? Players[1] : Players[0];

        public string ActivePlayer(int questionIndex) => Players[questionIndex % 2];

        public void ApplyOutcome(PvpAnswerOutcome outcome)
        {
            CurrentOutcome = outcome;
            ElapsedTotals[outcome.UserId] += outcome.ElapsedMs;
            if (outcome.IsCorrect)
            {
                Scores[outcome.UserId]++;
            }
        }

        public string? DetermineWinner()
        {
            var first = Players[0];
            var second = Players[1];

            if (Scores[first] != Scores[second])
            {
                return Scores[first] > Scores[second] ? first : second;
            }
            if (ElapsedTotals[first] != ElapsedTotals[second])
            {
                return ElapsedTotals[first] < ElapsedTotals[second] ? first : second;
            }
            return null;
        }
    }
}
