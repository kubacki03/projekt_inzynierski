namespace projekt_inzynierski.Server.Challenges.Infrastructures.Persistance
{
    public class UserChallenge
    {
        public int Id { get; set; }
        public bool Completed { get; set; }
        public int ChallengeId { get; set; }
        public string UserId { get; set; }
        public Challenge Challenge { get; set; }
    }
}