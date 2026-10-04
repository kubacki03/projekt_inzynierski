using projekt_inzynierski.Server.Courses.Domain.Models;
using projekt_inzynierski.Server.Courses.Domain.Repositories;
using Microsoft.EntityFrameworkCore;


namespace projekt_inzynierski.Server.Courses.Infrastructures.Persistance
{
    public class FeaturedCoursesRepository : IFeaturedCourseRepository
    {

        private readonly CourseDbContext _context;

        public FeaturedCoursesRepository(CourseDbContext context)
        {
            _context = context;
        }

        public List<FeaturedCourse> GetFeaturedCourses()
        {  
            return _context.FeaturedCourses.Include(p=>p.Course).ToList();
        }
         
        public async Task<FeaturedCourse> GetFeaturedCourse()
        {
            return await _context.FeaturedCourses.Include(x => x.Course).FirstOrDefaultAsync();
        }
    }
}
