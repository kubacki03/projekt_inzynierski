using projekt_inzynierski.Server.Courses.Application.DTOs;
using projekt_inzynierski.Server.Courses.Domain.Models;
using projekt_inzynierski.Server.Users.Application.DTOs;

namespace projekt_inzynierski.Server.Courses.Application.Interfaces
{
    public interface ICourseService
    {
        Task<Course> CreateNewCourse(string title, string description, string level, string language, string userId,string? path);
        Task<Course> CreateNewCourseFromNotification(string title, string description, string level, string language, string userId, int motherCourse);
        Task<PagedResult<CourseDto>> GetPagedCourses(int pageNumber, int pageSize); 
        Task<PagedResult<CourseDto>> GetPagedCoursesByTitle(string title, int pageNumber, int pageSize); 
        Task<int> CreatePrivateAdaptiveCourse(string userId, string technologies, string description,string path);
        Task<PagedResult<CourseDto>> GetPagedCoursesByLanguage(string language, int pageNumber, int pageSize);
    }
}
