using projekt_inzynierski.Server.Users.Domain.Models;

namespace projekt_inzynierski.Server.Courses.Application.DTOs
{
    public class AdaptiveCourseDto
    {
        public string Technologies { get; set; }
        public string Description { get; set; }
        public User User { get; set; }
        public string PdfPath { get; set; }
    }
}
