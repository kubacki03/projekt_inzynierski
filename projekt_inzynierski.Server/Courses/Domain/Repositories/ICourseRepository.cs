using projekt_inzynierski.Server.Courses.Application.DTOs;
using projekt_inzynierski.Server.Courses.Domain.Models;
using projekt_inzynierski.Server.Users.Application.DTOs;

namespace projekt_inzynierski.Server.Courses.Domain.Repositories
{
    public interface ICourseRepository
    {
        Course GetCourseById(int id);
        Task<Course> GetCourseByIdAsync(int id);
        Task<int> CreateNewCourse(Course c);
        List<Course> GetAllByIdFromList(List<int> listId);
        Task<int> CreateNewCourseForUser(string userId, Course c);
        Task<PagedResult<CourseDto>> GetPagedCourses(int pageNumber, int pageSize); 
        Task<PagedResult<CourseDto>> GetPagedCoursesByLanguage(string language, int pageNumber, int pageSize); 
        Task<PagedResult<CourseDto>> GetPagedCoursesByTitle(string title, int pageNumber, int pageSize);
    }
}
