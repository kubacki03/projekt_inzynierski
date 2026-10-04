

namespace projekt_inzynierski.Server.Challenges.Infrastructures.Persistance
{
    public class Challenge
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string RewardDescription { get; set; }
        public int BadgeId { get; set; }
        public int Points { get; set; }
        public string Type { get; set; }
        public List<UserChallenge> UserChallenges { get; set; } = new List<UserChallenge>();
    }
}