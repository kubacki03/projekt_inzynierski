namespace projekt_inzynierski.Server.Achievments.Domain.Models
{
    public class Achievement
    {
        public string Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public int Points { get; set; }
        public string RuleKey { get; set; } = default!;       
        public string RuleConfigJson { get; set; } = "{}";   
        public string BadgeType { get; set; }
        public string SkillCategory { get; set; }
    }
}
