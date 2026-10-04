using projekt_inzynierski.Server.Courses.Application.DTOs;
using projekt_inzynierski.Server.Courses.Application.Interfaces;
using projekt_inzynierski.Server.Courses.Domain.Repositories;

namespace projekt_inzynierski.Server.Courses.Infrastructures.Services
{
    public class FeaturedCourseService : IFeaturedCourses
    {
        private IFeaturedCourseRepository _courseRepository;
        public FeaturedCourseService(IFeaturedCourseRepository featuredCourseRepository) {
        this._courseRepository = featuredCourseRepository;
        }

        public List<CourseDto> GetFeaturedCourses()
        {
            var list = _courseRepository.GetFeaturedCourses();
            var listDto = new List<CourseDto>();
            list.ForEach(c => listDto.Add(new CourseDto { Title=c.Course.Title, Description=c.Course.Description, Language=c.Course.Language, Id=c.CourseId, Image=c.Course.ImageURL}));
            return listDto;
        }
         
        public async Task<CourseDto> GetFeaturedCourse()
        {
            var fc =await _courseRepository.GetFeaturedCourse();

            CourseDto dto = new CourseDto { Description = fc.Course.Description, Image = fc.Course.ImageURL, Language = fc.Course.Language, Title = fc.Course.Title,Level=fc.Course.Level };
            return dto;
        }
    }
}
