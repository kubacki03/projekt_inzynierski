namespace projekt_inzynierski.Server.Achievments.Domain.Models
{
    public class StudyDay
    {
        public Guid UserId { get; set; }
        public DateOnly Day { get; set; }
    }
}
