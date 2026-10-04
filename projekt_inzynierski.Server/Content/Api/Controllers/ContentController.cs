using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using projekt_inzynierski.Server.AIHelper.Application.DTOs;
using projekt_inzynierski.Server.AIHelper.Application.Interfaces;
using projekt_inzynierski.Server.Content.Application.DTOs;
using projekt_inzynierski.Server.Content.Application.Interfaces;
using projekt_inzynierski.Server.Content.Domain.Models;
using projekt_inzynierski.Server.Courses.Application.Interfaces;
using projekt_inzynierski.Server.Users.Application.Interfaces;
using RouteAttribute = Microsoft.AspNetCore.Components.RouteAttribute;

namespace projekt_inzynierski.Server.Content.Api.Controllers
{
    [ApiController]

    public class ContentController : Controller
    {

        ISubject _subjectService;
        IExercise _exerciseService;
        IUserCourses _userCourses;
        IAuthService _userService;

        public ContentController(ISubject subjectService, IUserCourses _courseService, IExercise exerciseService, IAuthService authService)
        {
            _userService = authService;
            _subjectService = subjectService;
            _userCourses = _courseService;
            _exerciseService = exerciseService;

        }

        [HttpGet("GetSubjects/{courseId}")]
        [Authorize]
        public async Task<IActionResult> GetSubjectsInCourse(int courseId)
        {
            var userId = User.Identity?.Name;
            var subjects = await _subjectService.GetSubjects(courseId, userId);

            var user = await _userService.GetUserByPublicIdAsync(userId);
            if (subjects == null || !subjects.Any())
            {
                var course = _userCourses.GetCourseById(courseId);
                try
                {
                    await _subjectService.GenerateSubjects(course.Title, course.Description, courseId, user);
                    subjects = await _subjectService.GetSubjects(courseId, userId);
                }
                catch
                {
                    return NotFound();
                }
            }

            return Ok(subjects);
        }



        [HttpGet("GetTheory/{subjectId}")]
        [Authorize]
        public async Task<IActionResult> GetTheory(int subjectId)
        {

            var list = await _subjectService.GetTheory(subjectId);
            if (list != null || list.Any())
            {
                return Ok(list);
            }
            return Ok();
        }

        [HttpGet("GetExercise/{subjectId}")]
        [Authorize]
        public async Task<IActionResult> GetExercises(int subjectId)
        {
            var userId = User.Identity?.Name;


            var list = await _subjectService.GetUserExercise(subjectId, userId);

            if (list != null && list.Any())
            {
                return Ok(list);
            }

            list = await _subjectService.GetUserExercise(subjectId, userId);
            return list.Any() ? Ok(list) : NotFound();
        }


        [HttpGet("GetQuiz/{subjectId}")]
        [Authorize]
        public async Task<IActionResult> GetQuiz(int subjectId)
        {
            var userId = User.Identity?.Name;

            List<Quiz> list = await _subjectService.GetUserQuiz(subjectId, userId);

            if (list != null && list.Any())
            {
                var list2 = list.Select(q => new QuizDto
                {
                    Id = q.Id,
                    Question = q.Question,
                    Answers = q.Answers.Select(a => new AnswerDto
                    {
                        Id = a.Id,
                        Text = a.Text,
                        IsCorrect = a.IsTrue
                    }).ToList(),
                    IsCompleted = q.Done
                }).ToList();

                return Ok(list2);
            }

            return NotFound();
        }

        [HttpPost("CheckIfContnentIsGenerated/{subjectId}")]
        [Authorize]
        public async Task<IActionResult> GenerateContent(int subjectId)
        {
            var userId = User.Identity?.Name;

            List<Quiz> list = await _subjectService.GetUserQuiz(subjectId, userId);

            if (list == null || !list.Any())
            {
                var user = await _userService.GetUserByPublicIdAsync(User.Identity?.Name);
                ContentModel response = await _subjectService.GenerateContent(userId, subjectId, user);
            }
            return Ok();
        }

        [HttpGet("GetExerciseById/{exerciseId}")]
        [Authorize]
        public async Task<IActionResult> GetExercise(int exerciseId)
        {
            var response = await _subjectService.GetExerciseById(exerciseId);
            var lang = await _exerciseService.GetLanguageBySubjectId(response.SubjectId);
            ExDto exDto = new ExDto { Task = response.Task, SubjectId = response.SubjectId, Id = response.Id, Language = lang };

            return Ok(exDto);
        }

        [HttpPost("GenerateTheoryFromPdf/{subjectId}/{courseId}")]
        [Authorize]
        public async Task<IActionResult> GenerateTheoryFromPdf(
            int subjectId,
            int courseId,
            IFormFile pdfFile) 
        {
            if (pdfFile == null || pdfFile.Length == 0)
                return BadRequest("Brak pliku PDF.");

            var userId = User.Identity?.Name;
            var user = await _userService.GetUserByPublicIdAsync(userId);
            var course = _userCourses.GetCourseById(courseId);
             
            var tempFilePath = Path.GetTempFileName();
            using (var stream = System.IO.File.Create(tempFilePath))
            {
                await pdfFile.CopyToAsync(stream);
            }

            var theory = await _subjectService.GenerateTheoryFromPdf(
                subjectId,
                course.Title,
                course.Description,
                user,
                tempFilePath 
            );
             
            System.IO.File.Delete(tempFilePath);

            return Ok(new TheoryDto
            {
                Id = theory.Id,
                Name = theory.Name,
                Content = theory.Content
            });

        }

    }
    public class TheoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Content { get; set; } = "";
    }

}
