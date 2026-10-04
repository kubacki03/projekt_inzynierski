namespace projekt_inzynierski.Server.Achievments.Domain.Models
{
    public class UserAchievement
    {
        public Guid UserId { get; set; }
        public string AchievementId { get; set; } = default!;
        public DateTime EarnedAtUtc { get; set; } = DateTime.UtcNow;

        public Achievement? Achievement { get; set; }
    }
}
