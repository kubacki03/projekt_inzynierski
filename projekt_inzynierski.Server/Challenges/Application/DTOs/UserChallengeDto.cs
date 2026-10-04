namespace projekt_inzynierski.Server.Challenges.Application.DTOs
{
    public class UserChallengeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string RewardDescription { get; set; }
        public int Points { get; set; }
        public string Status { get; set; }
        public bool isDone { get; set; }
        public bool isWeekly { get; set; }
        public string Type { get; set; }
    }
}