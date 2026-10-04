namespace projekt_inzynierski.Server.Users.Application.DTOs
{
    public class NotificationDto
    {
        public string Title { get; set; } = default!;
        public string Message { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string Type { get; set; } = "info";
        public int? RelatedCourseId { get; set; }
        public string CourseTitle { get; set; }
    }
}