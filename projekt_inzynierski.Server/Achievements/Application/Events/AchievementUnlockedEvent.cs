using MediatR;

namespace projekt_inzynierski.Server.Achievments.Application.Events
{
    public record AchievementUnlockedEvent : DomainEvent, INotification
    {
        public AchievementUnlockedEvent(Guid userId, string achievementId, DateTime earnedAtUtc)
        {
            UserId = userId;
            AchievementId = achievementId;
            OccurredAtUtc = earnedAtUtc;
        }

        public string AchievementId { get; init; } = default!;
    }

}
