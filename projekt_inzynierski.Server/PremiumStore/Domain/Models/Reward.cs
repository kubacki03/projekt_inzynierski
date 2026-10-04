namespace projekt_inzynierski.Server.PremiumStore.Domain.Models
{
    public class Reward
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Cost { get; set; } 
        public string ImageUrl { get; set; } 
        public bool IsActive { get; set; } = true;

        public string Type { get; set; } 

    }

}