namespace projekt_inzynierski.Server.Users.Domain.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public User User { get; set; }
        public Guid UserPublicId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public string SuggestedCourse { get; set; }
        public int MotherCourse { get; set; }
    }
}
