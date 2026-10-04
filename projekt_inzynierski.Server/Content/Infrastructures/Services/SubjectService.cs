using Microsoft.AspNetCore.Http.HttpResults;
using projekt_inzynierski.Server.Admin.Application.DTOs;
using projekt_inzynierski.Server.AIHelper.Application.DTOs;
using projekt_inzynierski.Server.AIHelper.Application.Interfaces;
using projekt_inzynierski.Server.Content.Application.DTOs;
using projekt_inzynierski.Server.Content.Application.Interfaces;
using projekt_inzynierski.Server.Content.Domain.Models;
using projekt_inzynierski.Server.Content.Domain.Repositories;
using projekt_inzynierski.Server.Courses.Domain.Models;
using projekt_inzynierski.Server.Courses.Domain.Repositories;
using projekt_inzynierski.Server.Users.Domain.Models;

namespace projekt_inzynierski.Server.Content.Infrastructures.Services
{
    public class SubjectService : ISubject
    {
        IAiGenerator _aiGenerator;
        ISubjectRepository _subjectRepository;
        ICourseRepository _courseRepository;
        IUserCourseRepository _userCourseRepository;
        
        public SubjectService(IAiGenerator aiGenerator, ISubjectRepository subjectRepository, ICourseRepository courseRepository, IUserCourseRepository userCourseRepository)
        {
            _userCourseRepository = userCourseRepository;
            _courseRepository = courseRepository;
            _aiGenerator = aiGenerator;
            _subjectRepository = subjectRepository;
        } 
         
        public async Task<ContentModel> GenerateContent(string userId,int subjectId, User user)
        { 
            if (!await _subjectRepository.DoesContentExistInSubject(subjectId))
            {
                var subject = await _subjectRepository.GetSubject(subjectId);
                var course = await _courseRepository.GetCourseByIdAsync(subject.CourseId);
                var x = await _userCourseRepository.GetUserCourseByUserIdAndCourseId(userId, course.Id); 
                var content = await _aiGenerator.GenerateContent(subject.Name, course.Title, course.Level,user, x.EvaluationResult.ToString()); 
                await _subjectRepository.AddContent(content, subjectId); 
                return content;
            }
            else
            {
                return null;
            }
        }

     

        public async Task<List<string>> GenerateSubjects(string courseName, string courseDescription, int courseId, User user)
        {
            var list = await _aiGenerator.GenerateSubjects(courseName,courseDescription,  user); 
            await _subjectRepository.AddSubjects(list,courseId, user.PublicId.ToString()); 
            return list;
        }

        public async Task<Exercise> GetExerciseById(int exerciseId)
        {
            return await _subjectRepository.GetExerciseById(exerciseId);
        }

        public async Task< List<SubjectDto>> GetSubjects(int courseId,string userId)
        {
            var sub =  _subjectRepository.GetSubjects(courseId);
            var dto = await _subjectRepository.GetProgressInList(sub, userId);
            return dto;
        }

        public async Task<List<Theory>> GetTheory(int subjectId)
        {
            return await _subjectRepository.GetTheories(subjectId);
        }

        public async Task<List<ExerciseDto>> GetUserExercise(int subjectId, string userId)
        {
           var exercises =  await _subjectRepository.GetUserExercise(subjectId);
           return exercises.Select(e => new ExerciseDto
            {
                Id = e.Id,
                Task = e.Task,
                SubjectId = e.SubjectId,
                IsDone = e.Done
            }).ToList();
        }

        public async Task<List<Quiz>> GetUserQuiz(int subjectId, string userId)
        {
            return await _subjectRepository.GetUserQuiz(subjectId, userId);
        }

        public async Task<float> GetUserMistakesCount(string userId, int subjectId)
        {
            return await _subjectRepository.GetUserMistakesCount(userId, subjectId);
        }

        public async Task<Theory> GenerateTheoryFromPdf(int subjectId,string courseName, string courseDescription, User user, string pdfPath)
        {
            var theory = await _aiGenerator.GenerateTheoryFromPdf(courseName, courseDescription, user, pdfPath);
            await _subjectRepository.AddTheoryToSubject(subjectId, theory); 
            return theory;
        } 
    }
}
