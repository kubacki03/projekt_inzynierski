using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using projekt_inzynierski.Server.AIHelper.Application.Interfaces;

namespace projekt_inzynierski.Server.AIHelper.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class AssistantChatController : ControllerBase
    {
        private readonly IChatAssistant chatAssistant;

        public AssistantChatController(IChatAssistant chatAssistant)
        {
            this.chatAssistant = chatAssistant;
        }

        [HttpPost("GetChatResponse")]
        [EnableRateLimiting("expensive")]
       
        public async Task<IActionResult> Post([FromBody] MessageRequest request)
        {
            var response = await chatAssistant.ChatResponse(request.Message);
            return Ok(new ApiResponse { Message=response, Success=true});
        }
    }

    public class MessageRequest
    {
        public string Message { get; set; }
    }
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
    }
