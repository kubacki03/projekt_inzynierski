using projekt_inzynierski.Server.Users.Domain.Models;

namespace projekt_inzynierski.Server.PremiumStore.Domain.Models
{
    public class UserReward
    {
        public int Id { get; set; }
        public Guid UserPublicId { get; set; }

        public int RewardId { get; set; }
        public Reward Reward { get; set; }
    }
}