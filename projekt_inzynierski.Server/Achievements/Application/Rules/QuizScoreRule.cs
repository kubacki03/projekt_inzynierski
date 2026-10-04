using projekt_inzynierski.Server.Achievments.Application.Events;
using projekt_inzynierski.Server.Achievments.Domain.Models;

namespace projekt_inzynierski.Server.Achievments.Application.Rules
{
    public class QuizScoreRule : IAchievementRule
    {
        public const string Key = "quiz-score";
        string IAchievementRule.Key => Key;

        public record Config
        {
            public int MinScore { get; init; } = 80; 
        }

        public async Task<bool> IsSatisfiedAsync(DomainEvent evt, Guid userId, Achievement a, AchievementsDbContext db, CancellationToken ct)
        {
            if (evt is not QuizCompletedEvent qEvt) return false;

            var cfg = RuleHelpers.ReadConfig<Config>(a) ?? new Config();
            return qEvt.Score >= cfg.MinScore;
        }
    }

}
