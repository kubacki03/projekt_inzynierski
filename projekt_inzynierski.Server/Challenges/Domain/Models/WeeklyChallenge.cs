namespace projekt_inzynierski.Server.Challenges.Infrastructures.Persistance
{
    public class WeeklyChallenge
    {
        public int Id { get; set; }
        public int ChallengeId { get; set; }
        public Challenge Challenge { get; set; }
    }
}