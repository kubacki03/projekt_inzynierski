using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using projekt_inzynierski.Server.Courses.Application.DTOs;
using projekt_inzynierski.Server.Courses.Application.Interfaces;
using projekt_inzynierski.Server.Courses.Domain.Models;
using projekt_inzynierski.Server.Courses.Infrastructures.Services;
using projekt_inzynierski.Server.Users.Application.DTOs;

namespace projekt_inzynierski.Server.Courses.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FeaturedCoursesController : Controller
    {
        private readonly IFeaturedCourses featuredCourseService;
        private readonly ICourseService _courseService;
        public FeaturedCoursesController(IFeaturedCourses service, ICourseService courseService)
        {
            _courseService = courseService;
            this.featuredCourseService = service;
        }

        [HttpGet("Get")]
        public IActionResult Get()
        {
            return Ok(featuredCourseService.GetFeaturedCourses());
        }

        [HttpGet("GetFeaturedCourse")]
        public async Task<IActionResult> GetFeaturedCourse()
        {
            var result = await featuredCourseService.GetFeaturedCourse();
            return Ok(result);
        }
        [HttpGet("paged")]
        public async Task<ActionResult<PagedResult<CourseDto>>> GetPagedCourses(
            int pageNumber = 1,
            int pageSize = 5)
        {
            return Ok(await _courseService.GetPagedCourses(pageNumber, pageSize));
        }

        [HttpGet("pagedByTitle")]
        public async Task<ActionResult<PagedResult<CourseDto>>> GetPagedByTitleCourses(
            string title,
            int pageNumber = 1,
            int pageSize = 5)
        {
            return Ok(await _courseService.GetPagedCoursesByTitle(title, pageNumber, pageSize));
        }

        [HttpGet("pagedByLanguage")]
        public async Task<ActionResult<PagedResult<CourseDto>>> GetPagedByLanguageCourses(
            string language,
            int pageNumber = 1,
            int pageSize = 5)
        {
            return Ok(await _courseService.GetPagedCoursesByLanguage(language, pageNumber, pageSize));
        }
    }
}
