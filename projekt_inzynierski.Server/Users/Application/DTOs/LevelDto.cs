namespace projekt_inzynierski.Server.Users.Application.DTOs
{
    public class LevelDto
    {
        public Levels Level { get; set; }
        public long MinimumPoints { get; set; }
        public long MaximumPoints { get; set; }
        public long TotalPoints { get; set; }
    }
}