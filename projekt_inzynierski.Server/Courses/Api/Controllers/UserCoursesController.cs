using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using projekt_inzynierski.Server.Courses.Application.DTOs;
using projekt_inzynierski.Server.Courses.Application.Interfaces;
using projekt_inzynierski.Server.Courses.Infrastructures.Services;
using projekt_inzynierski.Server.Users.Application.DTOs;
using projekt_inzynierski.Server.Users.Application.Interfaces;
using projekt_inzynierski.Server.Users.Domain.Models;

namespace projekt_inzynierski.Server.Courses.Api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class UserCoursesController : Controller
    {
        private readonly IUserCourses _courseService;
        private readonly INotificationService _notificationService;
        private readonly ICourseService _service;
        public UserCoursesController(IUserCourses courseService, INotificationService notificationservice, ICourseService service)
        {
            _notificationService = notificationservice;
            _courseService = courseService;
            _service = service;
        }
         
        [HttpGet("GetUserCourses")]
        [Authorize]
        public async Task<IActionResult> GetUserCourses()
        { 
            var userId = User.Identity?.Name; 
            if (userId == null)
            {
                return NotFound();
            }

            var courseList = await _courseService.GetUserCourses(userId);

            return Ok(courseList);
        }


        [HttpGet("IsUserInCourse")]
        [Authorize]
        public async Task<IActionResult> IsUserInCourse(int courseId)
        {
            var userId = User.Identity?.Name;
            var result = await _courseService.IsUserInCourse(userId, courseId);
            return Ok(result);
        }

        [HttpPost("JoinCourse")]
        [Authorize]
        public async Task<IActionResult> JoinCourse(int courseId)
        {
            var userId = User.Identity?.Name;

            if (userId == null)
            {
                return NotFound();
            } 

            try
            {
                await _courseService.JoinCourse(courseId.ToString(), userId);
            }
            catch
            {
                return Conflict();
            }
            return Ok();
        }

        [HttpPost("JoinCourseFromNotification")]
        [Authorize]
        public async Task<IActionResult> JoinCourseFromNotification(int notificationId)
        {
            var userId = User.Identity?.Name;

            if (userId == null)
            {
                return NotFound();
            } 

            var notification = await _notificationService.GetById(notificationId); 
            var course = await _service.CreateNewCourseFromNotification(notification.Title, "Kurs powtórzeniowy", "", "", userId, notification.MotherCourse);
            await _notificationService.DeleteById(notificationId); 
            return Ok();
        }


        [HttpPost("CreateAdaptiveCourse")]
        [Authorize]
        public async Task<IActionResult> CreateAdaptiveCourse([FromForm] AdaptiveDTO dto, IFormFile? pdf)
        {
            var userId = User.Identity?.Name; 
            string pdfPath = "";
            if (pdf != null)
            {
                var uploadDir = Path.Combine("wwwroot", "uploads");
                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }  
                pdfPath = Path.Combine(uploadDir, Guid.NewGuid() + Path.GetExtension(pdf.FileName));
                using (var stream = new FileStream(pdfPath, FileMode.Create))
                {
                    await pdf.CopyToAsync(stream);
                }
            }

            int courseId = await _service.CreatePrivateAdaptiveCourse(userId, dto.Technologies, dto.Description, pdfPath); 
            return Ok();
        }

    }

    public class AdaptiveDTO
    {
        public string Technologies { get; set; }
        public string Description { get; set; }
    }

}
