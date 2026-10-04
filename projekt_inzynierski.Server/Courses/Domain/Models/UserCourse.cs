

using projekt_inzynierski.Server.Courses.Domain.Events;

namespace projekt_inzynierski.Server.Courses.Domain.Models
{
    public class UserCourse
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public string UserId { get; set; } 
        public DateTime Joined { get; set; } 
        public LessonEvaluationResult EvaluationResult { get; set; } = LessonEvaluationResult.Normal; 
    }
}
