using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using projekt_inzynierski.Server.AIHelper.Application.Interfaces;

namespace projekt_inzynierski.Server.AIHelper.API.Controllers
{
    [ApiController]
    [Route("Learning")]
    public class LearningController : ControllerBase
    {
        private readonly IAdaptiveLearning _adaptiveLearning;

    public LearningController(IAdaptiveLearning adaptiveLearning)
        {
            _adaptiveLearning = adaptiveLearning;
        }

        public class CourseRequest
        {
            public string Technologies { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
        }

        [HttpPost("canBeCreated")]
        [Authorize]
        public async Task<IActionResult> CanBeCreated([FromBody] CourseRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Technologies) || string.IsNullOrWhiteSpace(request.Description))
                return BadRequest(new { success = false, message = "Brak wymaganych danych." });

            try
            {
                bool canBeCreated = await _adaptiveLearning.CanBeCreated(request.Technologies, request.Description);

                if (canBeCreated)
                    return Ok(new { success = true, message = "Kurs może zostać utworzony." });
                else
                    return StatusCode(418, new { success = false, message = "Kurs nie może zostać utworzony." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Błąd serwera: {ex.Message}" });
            }
        }
    }


}
