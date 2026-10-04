using Microsoft.AspNetCore.Mvc;
using projekt_inzynierski.Server.Courses.Application.Interfaces;

namespace projekt_inzynierski.Server.Courses.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PopularCoursesController : Controller
    {
        private readonly IUserCourses _service;

        public PopularCoursesController( IUserCourses service)
        {
            _service = service;
        }


        [HttpGet("Get")]
        public IActionResult Get()
        {
            return Ok(_service.GetMostPopularCourses());
        }

    }
}
