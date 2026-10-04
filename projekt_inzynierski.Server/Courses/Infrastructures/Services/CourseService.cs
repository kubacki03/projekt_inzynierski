using projekt_inzynierski.Server.AIHelper.Application.Interfaces;
using projekt_inzynierski.Server.Content.Domain.Repositories;
using projekt_inzynierski.Server.Courses.Application.DTOs;
using projekt_inzynierski.Server.Courses.Application.Interfaces;
using projekt_inzynierski.Server.Courses.Domain.Models;
using projekt_inzynierski.Server.Courses.Domain.Repositories;
using projekt_inzynierski.Server.Users.Application.DTOs;
using projekt_inzynierski.Server.Users.Domain.Models;

namespace projekt_inzynierski.Server.Courses.Infrastructures.Services
{
    public class CourseService : ICourseService
    {

        private readonly ICourseRepository _courseRepository;
        private readonly IAiGenerator _generatorService;
        private readonly ISubjectRepository _subjectRepository;
        public CourseService(ICourseRepository courseRepository, IAiGenerator aiGenerator, ISubjectRepository subjectRepository)
        {
            _subjectRepository = subjectRepository;
            _generatorService = aiGenerator;
            _courseRepository = courseRepository;
        }
        public async Task<Course> CreateNewCourse(string title, string description, string level, string language, string userId, string? path)
        {
            Course course = new Course
            {
                Title = title,
                Description = description,
                Language = language,
                Level = level,
                IsPublic = false,

                ImageURL = path

            };

            await _courseRepository.CreateNewCourse(course);
            await _courseRepository.CreateUserCourse(userId, course);
            return course;

        }

        public async Task<Course> CreateNewCourseFromNotification(string title, string description, string level, string language, string userId, int motherCourse)
        {
            var mc = _courseRepository.GetCourseById(motherCourse);
            Course course = new Course
            {
                Title = title,
                Description = "Kurs przypominający wiedzę",
                Language = mc.Language,
                Level = "Introduction",
                IsPublic = false

                ,
                ImageURL = "https://d1ub0o53i85pdh.cloudfront.net/uploads/2021/07/Facebook-Linkedin-image-template-3.jpg"
            };
            await _courseRepository.CreateNewCourse(course);
            await _courseRepository.CreateUserCourse(userId, course);
            return course;

        }

        public async Task<PagedResult<CourseDto>> GetPagedCourses(int pageNumber, int pageSize)
        {
            return await _courseRepository.GetPagedCourses(pageNumber, pageSize);
        }


        public async Task<PagedResult<CourseDto>> GetPagedCoursesByTitle(string title, int pageNumber, int pageSize)
        {
            return await _courseRepository.GetPagedCoursesByTitle(title, pageNumber, pageSize);
        }

        public async Task<PagedResult<CourseDto>> GetPagedCoursesByLanguage(string language, int pageNumber, int pageSize)
        {
            return await _courseRepository.GetPagedCoursesByLanguage(language, pageNumber, pageSize);
        }


        public async Task<int> CreatePrivateAdaptiveCourse(string userId, string technologies, string description, string pdfPath)
        {
            User user = new User();

            AdaptiveCourseDto courseDto = new AdaptiveCourseDto { User = user, Technologies = technologies, Description = description, PdfPath = pdfPath };
            var content = await _generatorService.GeneratePrivateSubjectsAndCourse(courseDto);
             
            var course = await CreateNewCourse(content.Course.Title, content.Course.Description, content.Course.Level, content.Course.Language, userId, "https://d1ub0o53i85pdh.cloudfront.net/uploads/2021/07/Facebook-Linkedin-image-template-3.jpg"); ;
             
            await _subjectRepository.AddSubjects(content.Subjects, course.Id, userId);

            var sub = await _subjectRepository.GetSubjectIdByTitleAndCourseId(content.FirstSubject, course.Id);
            if (content.Content != null)
            {
                await _subjectRepository.AddContent(content.Content, sub.Id);
            }
            return course.Id;

        }
    }
}
