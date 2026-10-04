
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using projekt_inzynierski.Server.Content.Application.Interfaces;
using projekt_inzynierski.Server.Courses.Application.Interfaces;
using projekt_inzynierski.Server.Users.Application.Interfaces;
using System.Threading.Tasks;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IAdminCourseService _courseService;
    private readonly IContentAdminService _contentService;
    private readonly IUserAdminService _userService;

    public AdminController(
        IAdminCourseService courseService,
        IContentAdminService contentService,
        IUserAdminService userService)
    {
        _courseService = courseService;
        _contentService = contentService;
        _userService = userService;
    }

  
    [HttpPost("courses")]

    public async Task<IActionResult> CreateCourse([FromBody] CreateCourseRequest request)
    {
      var x=  await _courseService.CreateCourse(
            request.Title,
            request.Description,
            request.IsPublic,
            request.Language,
            request.Level
        );
        return Ok(x);
    }

    [HttpGet("courses/count")]
   
    public async Task<IActionResult> GetCourseCount()
    {
        var count = await _courseService.GetCourseCount();
        return Ok(count);
    }


    [HttpGet("content/quiz/average")]
    public async Task<IActionResult> GetAverageQuizAttempts()
    {
        var avg = await _contentService.GetAverageQuizAttempts();
      
        return Ok(avg);
    }

    [HttpGet("content/exercise/average")]

    public async Task<IActionResult> GetAverageExerciseAttempts()
    {
        float avg = await _contentService.GetAverageExerciseAttempts();
        return Ok(avg);
    }


    [HttpGet("users/count")]

    public async Task<IActionResult> GetUserCount()
    {
        var count = await _userService.GetUserCount();
        return Ok(count);
    }

    [HttpPost("users/{id}/ban")]
 
    public async Task<IActionResult> BanUser(int id)
    {
        await _userService.BanUser(id);
        return Ok(new { Message = $"User {id} banned." });
    }

    [HttpPost("users/{id}/unban")]

    public async Task<IActionResult> UnbanUser(int id)
    {
        await _userService.UnbanUser(id);
        return Ok(new { Message = $"User {id} unbanned." });
    }

    [HttpGet("users")]

    public async Task<IActionResult> GetUsersPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _userService.GetUsersPagedAsync(page, pageSize);
        return Ok(result);
    }
}


public class CreateCourseRequest
{
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsPublic { get; set; }
    public string Language { get; set; }
    public string Level { get; set; }
}

