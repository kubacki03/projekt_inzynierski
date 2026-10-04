using projekt_inzynierski.Server.Courses.Domain.Models;

namespace projekt_inzynierski.Server.Courses.Domain.Repositories
{
    public interface IFeaturedCourseRepository
    {
        Task<FeaturedCourse> GetFeaturedCourse(); 
        List<FeaturedCourse> GetFeaturedCourses();
    }
}
