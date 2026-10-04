using projekt_inzynierski.Server.Users.Domain.Models;

namespace projekt_inzynierski.Server.PremiumStore.Domain.Models
{
    public class UserAvatar
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int AvatarId { get; set; }
        public Avatar Avatar { get; set; }

        public DateTime UnlockedAt { get; set; } = DateTime.UtcNow;

        public bool IsSelected { get; set; } 
    }

}