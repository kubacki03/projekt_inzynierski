namespace projekt_inzynierski.Server.Courses.Domain.Events
{
    using MediatR;

    public class LessonEvaluatedEvent : INotification
    {
        public string UserId { get; }
        public float Errors { get; }
        public int CourseId { get; } 
        public List<string> problems { get; }
        public LessonEvaluationResult Result { get; } 
        public LessonEvaluatedEvent(string userId, float errors, int courseId, List<string> p, LessonEvaluationResult result)
        {
            problems = p;
            Errors = errors;
            UserId = userId;
            CourseId = courseId; 
            Result = result;
        }

    } 
    public enum LessonEvaluationResult
    {
        Normal,
        EasierNextLesson,
        SuggestRevisionCourse,
        HarderNextLesson
    }
}
