using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projekt_inzynierski.Server.Courses.Application.DTOs;
using projekt_inzynierski.Server.Courses.Domain.Repositories;
using projekt_inzynierski.Server.Users.Application.DTOs;

namespace projekt_inzynierski.Server.Courses.Infrastructures.Persistance
{
    public class CourseAdminRepository: IAdminCourseRepository
    {
        private readonly CourseDbContext _context;
        public CourseAdminRepository(CourseDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetCourseCount()
        {
            return await _context.Courses.CountAsync();
        } 
    }
}
