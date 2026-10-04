using Microsoft.EntityFrameworkCore;
using projekt_inzynierski.Server.Courses.Domain.Events;
using projekt_inzynierski.Server.Courses.Domain.Models;
using projekt_inzynierski.Server.Courses.Domain.Repositories;


namespace projekt_inzynierski.Server.Courses.Infrastructures.Persistance
{
    public class UserCourseRepository : IUserCourseRepository
    {
        private readonly CourseDbContext _context;

        public UserCourseRepository(CourseDbContext context)
        {
            _context = context;
        }

        public async Task AddUserToCourse(UserCourse userCourse)
        {
          await _context.UserCourses.AddAsync(userCourse);
          await _context.SaveChangesAsync();
        }

        public List<Course> GetMostPopularCourses()
        {
            return _context.UserCourses
             
                .GroupBy(uc => uc.CourseId)
                .OrderByDescending(g => g.Count())
                .Take(10)
                
                .Select(g => g.Key)
                
                .Join(_context.Courses, id => id, c => c.Id, (id, course) => course)
                .Where(x=>x.IsPublic==true)
                .ToList();
        }
         
        public async Task<List<UserCourse>> GetUserCoursesByUserId(string userId)
        {
            return await _context.UserCourses.Include(x=>x.Course).Where(a => a.UserId == userId).ToListAsync();
        }

        public Task<bool> IsUserInCourse(int courseId, string userId)
        {
            return _context.UserCourses
                .AnyAsync(a => a.UserId == userId && a.CourseId == courseId);
        }

        public async Task SetEvaluationResult(LessonEvaluationResult result, string userId, int courseId)
        { 
            var x = await _context.UserCourses.FirstOrDefaultAsync(x=>x.UserId==userId && x.CourseId==courseId);
            x.EvaluationResult = result;
            await _context.SaveChangesAsync();
        }

        public async Task<UserCourse> GetUserCourseByUserIdAndCourseId(string userId, int courseId)
        {
            return await _context.UserCourses.FirstOrDefaultAsync(x => x.UserId == userId && x.CourseId == courseId);
        }

    }
}
