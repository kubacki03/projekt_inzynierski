namespace projekt_inzynierski.Server.Achievments.Application.DTOs
{
    public class AchievmentDto
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public string Level { get; set; }
        public string Category { get; set; }
        public DateTime Date { get; set; }

        public int Progress { get; set; }
    }
}
