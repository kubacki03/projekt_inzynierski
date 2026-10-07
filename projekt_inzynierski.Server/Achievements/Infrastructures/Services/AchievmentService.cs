using projekt_inzynierski.Server.Achievments.Application.Events;
using projekt_inzynierski.Server.Achievments.Application.Interfaces;
using projekt_inzynierski.Server.Achievments.Application.Rules;
using projekt_inzynierski.Server.Achievments.Domain.Models;
using System.Text.Json.Serialization;
using System.Text.Json;
using System;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace projekt_inzynierski.Server.Achievments.Infrastructures.Services
{
    public class AchievementService : IAchievementService
    {
        private readonly AchievementsDbContext _db;
        private readonly IEnumerable<IAchievementRule> _rules;
        private readonly IPublisher _publisher;

        private static readonly JsonSerializerOptions JsonOpts = new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

        public AchievementService(AchievementsDbContext db, IEnumerable<IAchievementRule> rules, IPublisher publisher)
        {
            _db = db; _rules = rules; _publisher = publisher;
        }

        public async Task HandleEventAsync(DomainEvent evt, CancellationToken ct = default)
        {
            
            switch (evt)
            {
                case LessonCompletedEvent l:
                    if (!await _db.UserProgress.AnyAsync(p => p.UserId == l.UserId && p.LessonId == l.LessonId, ct))
                    {
                        _db.UserProgress.Add(new UserProgress
                        {
                            UserId = l.UserId,
                            LessonId = l.LessonId,
                            Language = l.Language,
                            CompletedAtUtc = l.OccurredAtUtc
                        });
                    }
                    break;

                case StudyLoggedEvent s:
                    if (!await _db.StudyDays.AnyAsync(d => d.UserId == s.UserId && d.Day == s.Day, ct))
                    {
                        _db.StudyDays.Add(new StudyDay { UserId = s.UserId, Day = s.Day });
                    }
                    break;
            }

            await _db.SaveChangesAsync(ct);

          
            var candidateAchievements = await _db.Achievements.ToListAsync(ct);
            int count = _rules.Count();
           
            var rulesByKey = _rules.ToDictionary(r => r.Key);

            foreach (var a in candidateAchievements)
            {
                if (!rulesByKey.TryGetValue(a.RuleKey, out var rule)) continue;

              
                bool already = await _db.UserAchievements.AnyAsync(ua => ua.UserId == evt.UserId && ua.AchievementId == a.Id, ct);
                if (already) continue;

                bool ok = await rule.IsSatisfiedAsync(evt, evt.UserId, a, _db, ct);
                if (!ok) continue;

        
                var earned = new UserAchievement { UserId = evt.UserId, AchievementId = a.Id, EarnedAtUtc = DateTime.UtcNow };
                _db.UserAchievements.Add(earned);
                await _db.SaveChangesAsync(ct);

                await _publisher.Publish(new AchievementUnlockedEvent(evt.UserId, a.Id, earned.EarnedAtUtc), ct);
            }
        }
    }
}
