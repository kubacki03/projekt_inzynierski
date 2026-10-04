using projekt_inzynierski.Server.Content.Domain.Models;
using projekt_inzynierski.Server.Courses.Domain.Models;

namespace projekt_inzynierski.Server.AIHelper.Application.DTOs
{
    public class AdaptiveCourseWithSubjectsAndContent
    {
        public ContentModel Content { get; set; }
        public List<string> Subjects { get; set; } = new List<string>();
        public Course Course { get; set; }
        public string FirstSubject { get; set; }
    }
}
