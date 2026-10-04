using projekt_inzynierski.Server.Courses.Application.DTOs;
using projekt_inzynierski.Server.Courses.Application.Interfaces;
using projekt_inzynierski.Server.Courses.Domain.Models;
using projekt_inzynierski.Server.Courses.Domain.Repositories;
using projekt_inzynierski.Server.Courses.Infrastructures.Persistance;

namespace projekt_inzynierski.Server.Courses.Infrastructures.Services
{
    public class UserCourseService : IUserCourses
    {
        private readonly IUserCourseRepository _repository;
        private readonly ICourseRepository _courseRepository;
        public UserCourseService(IUserCourseRepository repository, ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
            _repository = repository;
        }

        public Course GetCourseById(int courseId)
        {
            return _courseRepository.GetCourseById(courseId);
        }

        public List<CourseDto> GetMostPopularCourses()
        {
           var list=  _repository.GetMostPopularCourses();
            List<CourseDto> courseDtos = new List<CourseDto>();
            list.ForEach(a => courseDtos.Add(new CourseDto {Id=a.Id, Title = a.Title, Description = a.Description, Language = a.Language, Image=a.ImageURL }));
            return courseDtos;
        }

        public async Task<List<UserCourseDTO>> GetUserCourses(string userId)
        {
            var userCourses =await _repository.GetUserCoursesByUserId(userId);
            List<UserCourseDTO> listDto = new List<UserCourseDTO>();

            userCourses.ForEach(a => listDto.Add(new UserCourseDTO { Name = a.Course.Title, Description = a.Course.Description, CreatedDate = a.Joined, Image = a.Course.ImageURL, Id=a.CourseId }));

            return listDto;
        }

        public async Task<bool> IsUserInCourse(string userId, int courseId)
        {
            return await _repository.IsUserInCourse(courseId, userId);
        }

        public async Task JoinCourse(string courseId,string userId)
        {
            int.TryParse(courseId, out int intCourseId);
            if (await IsUserInCourse( userId,  intCourseId))
            {
                throw new Exception("user is in course");
            }
            UserCourse userCourse = new UserCourse { CourseId= intCourseId, Joined=DateTime.Now, UserId=userId };
            await _repository.AddUserToCourse(userCourse);
           
        }
    }
}
