namespace projekt_inzynierski.Server.Achievments.Domain.Models
{
    public class UserProgress
    {
        public Guid UserId { get; set; }
        public string LessonId { get; set; } = default!;
        public string Language { get; set; } = default!;
        public DateTime CompletedAtUtc { get; set; }
    }
}
