using projekt_inzynierski.Server.Courses.Application.DTOs;

namespace projekt_inzynierski.Server.Courses.Application.Interfaces
{
    public interface IFeaturedCourses
    {
        public List<CourseDto> GetFeaturedCourses();
        Task<CourseDto> GetFeaturedCourse();
    }
}
