using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using projekt_inzynierski.Server.PremiumStore.Application.Repositories;
using projekt_inzynierski.Server.Users.Application.Commands;
using projekt_inzynierski.Server.Users.Application.DTOs;
using projekt_inzynierski.Server.Users.Application.Interfaces;
using projekt_inzynierski.Server.Users.Infrastructures.Services;

namespace projekt_inzynierski.Server.Users.Api
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IAuthService _userService;
        private readonly IUserProgress _progressService;
        private readonly IRewardRepository _rewardService;
        public UserController(IMediator mediator, IAuthService userService, IUserProgress userProgress, IRewardRepository rewardService)
        {
            _mediator = mediator;
            _userService = userService;
            _progressService = userProgress;
            _rewardService = rewardService;
        }

        [HttpPost("Register")]
        [EnableRateLimiting("auth")]
        public async Task<IActionResult> Register(UserRegisterDto request)
        {
            try
            {
                await _mediator.Send(new RegisterUserCommand(request));
            }
            catch (DbUpdateException)
            {
                // Unique index on e-mail: report a conflict instead of a 500.
                return Conflict(new { message = "Nie można utworzyć konta z podanymi danymi" });
            }
            return await Login(new UserLoginDto { email = request.email, password = request.password });


        }
        [HttpPost("Login")]
        [EnableRateLimiting("auth")]
        public async Task<IActionResult> Login(UserLoginDto request)
        {
            if (string.IsNullOrWhiteSpace(request.email) || string.IsNullOrWhiteSpace(request.password))
                return BadRequest("Email and password are required");

            string token = "";
            string role = "user";

            if (request.email.Contains("@codeoddysey"))
            {
                try
                {
                    token = await _mediator.Send(new LoginAdminCommand(request.email, request.password));
                    role = "admin"; 
                }
                catch (UnauthorizedAccessException)
                {
                    return Unauthorized(new { message = "Nieprawidłowy e-mail lub hasło" });
                }
            }
            else
            {
                try
                {
                    token = await _mediator.Send(new LoginUserCommand(request.email, request.password));
                }
                catch (UnauthorizedAccessException ex)
                {
                    return Unauthorized(new { message = ex.Message });
                }
            }

            Response.Cookies.Append("access_token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(1)
            });
             
            return Ok(new { role });
        }


        [HttpGet("IsAuthenticated")]
        public IActionResult IsAuthenticated()
        {
            var x = User.Identity?.IsAuthenticated ?? false;
            return Ok(x);
        }



        [HttpGet("IsAdmin")]
        public IActionResult CheckRole()
        {
            
            if (User.IsInRole("Admin"))
            {
                return Ok("admin");
            }
            else if (User.IsInRole("User"))
            {
                return Ok("user");
            }

            return BadRequest("Nie znaleziono roli");
        }

        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("access_token", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });

            return Ok();
        }


        [HttpGet("Progress")]
        [Authorize]
        public async Task<IActionResult> UserProgress()
        {
            var publicId = User.Identity?.Name;

            var response = await _progressService.GetUserProgressAsync(publicId);


            return Ok(response);
        }

        [HttpGet("GetMostActive")]
        public async Task<IActionResult> GetMostActive()
        {
            var publicId = User.Identity?.Name;

            var response = await _progressService.GetMostActive();


            return Ok(response);
        }


        [HttpGet("GetGoldenPoints")]
        [Authorize]
        public async Task<IActionResult> GetGoldenPoints()
        {
            var publicId = User.Identity?.Name;

            var response = await _userService.GetUserByPublicIdAsync(publicId);

            if (response == null)
            {
                return NotFound();
            }
            return Ok(response.GoldenPoints);
        }


        [HttpGet("Get")]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var publicId = User.Identity?.Name;

            var user = await _userService.GetUserByPublicIdAsync(publicId);

            UserDto userDto = new UserDto
            {
                FirstName = user.FirstName,
                BirthDate = user.BirthDate,
                SelectedAvatar = (await _rewardService.GetAvatarById(user.SelectedAvatarId)).ImageUrl,
                EducationLevel = user.EducationLevel,
                Email = user.Email,
                Experience = user.Experience,
                Gender = user.Gender,
                GoldenPoints = user.GoldenPoints,
                Nickname = user.Nickname,
                Points = user.Points,
                Date=user.AccountPremiumDateEnd

            };


            return Ok(userDto);
        }
    }
}