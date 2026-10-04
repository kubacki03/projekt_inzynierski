using Microsoft.EntityFrameworkCore;
using projekt_inzynierski.Server.Achievments.Application.Events;
using projekt_inzynierski.Server.Achievments.Domain.Models;
using System;

namespace projekt_inzynierski.Server.Achievments.Application.Rules
{
    public class LanguageExplorerRule : IAchievementRule
    {
        public const string Key = "language-explorer";
        string IAchievementRule.Key => Key;

        public record Config { public int DistinctLanguages { get; init; } = 2; }

        public async Task<bool> IsSatisfiedAsync(DomainEvent evt, Guid userId, Achievement a, AchievementsDbContext db, CancellationToken ct)
        {
            if (evt is not LessonCompletedEvent) return false;
            var cfg = RuleHelpers.ReadConfig<Config>(a) ?? new Config();
            var distinct = await db.UserProgress
                .Where(x => x.UserId == userId)
                .Select(x => x.Language.ToLower())
                .Distinct()
                .CountAsync(ct);

            return distinct >= cfg.DistinctLanguages;
        }
    }
}
