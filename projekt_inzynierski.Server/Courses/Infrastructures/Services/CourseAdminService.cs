using projekt_inzynierski.Server.Courses.Application.Interfaces;
using projekt_inzynierski.Server.Courses.Domain.Models;
using projekt_inzynierski.Server.Courses.Domain.Repositories;

namespace projekt_inzynierski.Server.Courses.Infrastructures.Services
{
    public class CourseAdminService : IAdminCourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IAdminCourseRepository _adminCourseRepository;
        public CourseAdminService(ICourseRepository courseRepository, IAdminCourseRepository adminCourseRepository)
        {
            this._courseRepository = courseRepository;
            _adminCourseRepository = adminCourseRepository;
        }

        public async Task<int> CreateCourse(string title,string description, bool isPublic,string language,string level)
        {
            Course course = new Course { Description = description, IsPublic = isPublic, Language =language, Level=level, Title=title};
            var x =await _courseRepository.CreateNewCourse(course);
            return x;
        }

        public async Task<int> GetCourseCount()
        {
            return await _adminCourseRepository.GetCourseCount();
        }
    }
}
