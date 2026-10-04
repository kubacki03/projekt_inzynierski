namespace projekt_inzynierski.Server.PremiumStore.Domain.Models
{
    public class AvatarReward
    {
        public int Id { get; set; }
        public int AvatarId { get; set; }
        public Avatar Avatar { get; set; }
        public int RewardId { get; set; }
    }
}