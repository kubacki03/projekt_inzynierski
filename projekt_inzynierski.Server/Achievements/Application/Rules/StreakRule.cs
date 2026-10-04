using Microsoft.EntityFrameworkCore;
using projekt_inzynierski.Server.Achievments.Application.Events;
using projekt_inzynierski.Server.Achievments.Domain.Models;
using System;

namespace projekt_inzynierski.Server.Achievments.Application.Rules
{
    public class StreakRule : IAchievementRule
    {
        public const string Key = "streak-days";
        string IAchievementRule.Key => Key;

        public record Config { public int Days { get; init; } = 7; }

        public async Task<bool> IsSatisfiedAsync(DomainEvent evt, Guid userId, Achievement a, AchievementsDbContext db, CancellationToken ct)
        {
            if (evt is not StudyLoggedEvent sEvt) return false;
            var cfg = RuleHelpers.ReadConfig<Config>(a) ?? new Config();

            int streak = 0;
            var day = sEvt.Day;
            while (await db.StudyDays.AnyAsync(x => x.UserId == userId && x.Day == day, ct))
            {
                streak++;
                day = day.AddDays(-1);
            }
            return streak >= cfg.Days;
        }
    }
}
