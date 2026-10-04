using Microsoft.EntityFrameworkCore;
using projekt_inzynierski.Server.Achievments.Application.Events;
using projekt_inzynierski.Server.Achievments.Domain.Models;
using System;

namespace projekt_inzynierski.Server.Achievments.Application.Rules
{
    
    public class LessonCountRule : IAchievementRule
    {
        public const string Key = "lesson-count";
        string IAchievementRule.Key => Key;

        public record Config
        {
            public int Threshold { get; init; } = 1;
        }

        public async Task<bool> IsSatisfiedAsync(DomainEvent evt, Guid userId, Achievement a, AchievementsDbContext db, CancellationToken ct)
        {
            if (evt is not LessonCompletedEvent) return false;
            var cfg = RuleHelpers.ReadConfig<Config>(a) ?? new Config();

            var count = await db.UserProgress.CountAsync(x => x.UserId == userId, ct);
            return count >= cfg.Threshold;
        }
    }
}
