using projekt_inzynierski.Server.Achievments.Application.Events;
using projekt_inzynierski.Server.Achievments.Domain.Models;
using System;

namespace projekt_inzynierski.Server.Achievments.Application.Rules
{
    public interface IAchievementRule
    {
        string Key { get; }
        Task<bool> IsSatisfiedAsync(DomainEvent evt, Guid userId, Achievement achievement, AchievementsDbContext db, CancellationToken ct);
    }
}
