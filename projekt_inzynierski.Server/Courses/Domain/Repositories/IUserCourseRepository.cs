using projekt_inzynierski.Server.Courses.Domain.Events;
using projekt_inzynierski.Server.Courses.Domain.Models;


namespace projekt_inzynierski.Server.Courses.Domain.Repositories
{
    public interface IUserCourseRepository
    {
        Task<List<UserCourse>> GetUserCoursesByUserId(string userId);
        List<Course> GetMostPopularCourses(); 
        Task AddUserToCourse(UserCourse userCourse); 
        Task<bool> IsUserInCourse(int courseId, string userId);
        Task SetEvaluationResult(LessonEvaluationResult result, string userId, int courseId);
        Task<UserCourse> GetUserCourseByUserIdAndCourseId(string userId, int courseId);
    }
}
