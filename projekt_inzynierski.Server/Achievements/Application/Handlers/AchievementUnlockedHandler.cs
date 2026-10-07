using MediatR;
using Microsoft.AspNetCore.SignalR;
using projekt_inzynierski.Server.Achievments.Application.Events;
using projekt_inzynierski.Server.Achievments.Infrastructures.Services;

namespace projekt_inzynierski.Server.Achievments.Application.Handlers
{
    // Pushes the unlocked achievement to the user's SignalR group.
    // Persisting the UserAchievement is done by AchievementService before the event is published.
    public class AchievementUnlockedHandler : INotificationHandler<AchievementUnlockedEvent>
    {
        private readonly IHubContext<AchievementsHub> _hub;
        private readonly ILogger<AchievementUnlockedHandler> _logger;

        public AchievementUnlockedHandler(IHubContext<AchievementsHub> hub, ILogger<AchievementUnlockedHandler> logger)
        {
            _hub = hub;
            _logger = logger;
        }

        public async Task Handle(AchievementUnlockedEvent notification, CancellationToken cancellationToken)
        {
            await _hub.Clients.Group($"user-{notification.UserId}")
                .SendAsync("AchievementUnlocked", new
                {
                    notification.UserId,
                    notification.AchievementId,
                    notification.OccurredAtUtc
                }, cancellationToken);

            _logger.LogInformation("Achievement {AchievementId} unlocked by {UserId}", notification.AchievementId, notification.UserId);
        }
    }
}
