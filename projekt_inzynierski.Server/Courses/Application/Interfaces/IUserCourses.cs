using projekt_inzynierski.Server.Courses.Application.DTOs;
using projekt_inzynierski.Server.Courses.Domain.Models;

namespace projekt_inzynierski.Server.Courses.Application.Interfaces
{
    public interface IUserCourses
    {
        Task<List<UserCourseDTO>> GetUserCourses(string userId); 
        List<CourseDto> GetMostPopularCourses(); 
        Task JoinCourse(string courseId, string userId);
        Task<bool> IsUserInCourse(string userId,int courseId); 
        Course GetCourseById(int courseId);
    }
}
