using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using projekt_inzynierski.Server.Content.Application.DTOs;
using projekt_inzynierski.Server.Content.Application.Interfaces;
using projekt_inzynierski.Server.Content.Infrastructures.Services;
using projekt_inzynierski.Server.Users.Application.Interfaces;
using projekt_inzynierski.Server.Users.Domain.Models;
using projekt_inzynierski.Server.Users.Infrastructures.Services;

namespace projekt_inzynierski.Server.Content.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuizController : Controller
    {
        private readonly IQuiz _quizService;
        private readonly IUserProgress _userProgressService;

        public QuizController(IQuiz quizService, IUserProgress ads)
        {
            _quizService = quizService;
            _userProgressService = ads;
        }

        [HttpPost("SaveAnswer")]
        [Authorize]
        public async Task<IActionResult> SaveQuizProgress([FromBody] SaveAnswerDto request, CancellationToken ct)
        {
            var user = User.Identity?.Name;

            if (string.IsNullOrEmpty(user))
            {
                return Unauthorized();
            }
             
            try
            {
               
                await _quizService.AddUserAnswer(user, request.QuizId,request.AnswerId, ct);
                await _userProgressService.IncreaseUserProgress(user, 50, ct);

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

    }

}
