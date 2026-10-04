using Microsoft.EntityFrameworkCore;
using projekt_inzynierski.Server.Courses.Application.DTOs;
using projekt_inzynierski.Server.Courses.Domain.Models;
using projekt_inzynierski.Server.Courses.Domain.Repositories;
using projekt_inzynierski.Server.Users.Application.DTOs;

namespace projekt_inzynierski.Server.Courses.Infrastructures.Persistance
{
    public class CourseRepository : ICourseRepository
    {

        private readonly CourseDbContext _context;

        public CourseRepository(CourseDbContext context)
        {
            _context = context;
        }

        public List<Course> GetAllByIdFromList(List<int> listId)
        {
            return _context.Courses.Where(a => listId.Contains(a.Id)).ToList();
        }

        public Course GetCourseById(int id)
        {

            return _context.Courses.FirstOrDefault(a => a.Id == id);
        }

        public async Task<Course> GetCourseByIdAsync(int id)
        {
            return await _context.Courses.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<int> CreateNewCourse(Course c)
        {
            await _context.Courses.AddAsync(c);
            await _context.SaveChangesAsync();
            return c.Id;

        }

        public async Task CreateUserCourse(string userId, Course c)
        {
            UserCourse userCourse = new UserCourse { Course = c, UserId = userId };
            await _context.UserCourses.AddAsync(userCourse);
            await _context.SaveChangesAsync();

        }



        public async Task<PagedResult<CourseDto>> GetPagedCourses(int pageNumber, int pageSize )
        {
            if (pageNumber <= 0)
            {
                pageNumber = 1;
            }
            if (pageSize <= 0)
            {
                pageSize = 5;
            }

            var query = _context.Courses.Where(x=>x.IsPublic==true).AsNoTracking();

            var totalCount = await query.CountAsync();

            var courses = await query
                .OrderBy(c => c.Id) 
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CourseDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    Language = c.Language,
                    Image = c.ImageURL,
                    Level = c.Level
                })
                .ToListAsync();

            return new PagedResult<CourseDto>
            {
                Items = courses,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<PagedResult<CourseDto>> GetPagedCoursesByTitle(string title, int pageNumber, int pageSize)
        {
            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0) pageSize = 5;

            var query = _context.Courses
                .Where(x => x.IsPublic && x.Title.Contains(title))
                .AsNoTracking();

            var totalCount = await query.CountAsync();

            var courses = await query
                .OrderBy(c => c.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CourseDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    Language = c.Language,
                    Image = c.ImageURL,
                    Level = c.Level
                })
                .ToListAsync();

            return new PagedResult<CourseDto>
            {
                Items = courses,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<PagedResult<CourseDto>> GetPagedCoursesByLanguage(string language, int pageNumber, int pageSize)
        {
            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0) pageSize = 5;

            var query = _context.Courses
                .Where(x => x.IsPublic && x.Language == language)
                .AsNoTracking();

            var totalCount = await query.CountAsync();

            var courses = await query
                .OrderBy(c => c.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CourseDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    Language = c.Language,
                    Image = c.ImageURL,
                    Level = c.Level
                })
                .ToListAsync();

            return new PagedResult<CourseDto>
            {
                Items = courses,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}