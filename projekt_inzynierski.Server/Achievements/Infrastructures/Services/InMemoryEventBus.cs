using projekt_inzynierski.Server.Achievments.Application.Events;
using projekt_inzynierski.Server.Achievments.Application.Interfaces;

namespace projekt_inzynierski.Server.Achievments.Infrastructures.Services
{
    using Microsoft.AspNetCore.SignalR;
    using projekt_inzynierski.Server.Achievments.Domain.Models;

    public class InMemoryEventBus : IEventBus
    {
        private readonly IHubContext<AchievementsHub> _hub;
        private readonly IUserAchievment _userAchievment;
        public InMemoryEventBus(IHubContext<AchievementsHub> hub, IUserAchievment userAchievment)
        {
            _hub = hub;
            _userAchievment = userAchievment;
        }

        public async Task PublishAsync<T>(T evt, CancellationToken ct = default) where T : DomainEvent
        {
            if (evt is AchievementUnlockedEvent unlocked)
            {
         
                await _hub.Clients.Group($"user-{unlocked.UserId}")
                    .SendAsync("AchievementUnlocked", new
                    {
                        unlocked.UserId,
                        unlocked.AchievementId,
                        unlocked.OccurredAtUtc
                    }, ct);

                UserAchievement us = new UserAchievement { AchievementId = unlocked.AchievementId, EarnedAtUtc = DateTime.UtcNow, UserId = unlocked.UserId };
               await _userAchievment.SaveUserAchievement(us);
            }

           

            Console.WriteLine($"[EVENT] AchievementUnlocked -> user={evt.UserId} type={typeof(T).Name}");
        }
    }

}
