namespace projekt_inzynierski.Server.PremiumStore.Domain.Models
{
    public class Avatar
    {
        public int Id { get; set; }
        public string Name { get; set; }     
        public string ImageUrl { get; set; } 

        public bool IsActive { get; set; } = true;

        public ICollection<UserAvatar> UserAvatars { get; set; }
    }

}