using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using projekt_inzynierski.Server.Users.Application.Interfaces;
using projekt_inzynierski.Server.Users.Infrastructures.Services;


namespace projekt_inzynierski.Server.Users.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly INotificationService _notificationService;
        public NotificationController(IHubContext<NotificationHub> hubContext, INotificationService notificationService)
        {
            _notificationService = notificationService;
            _hubContext = hubContext;
        }

        [HttpPost("sendToUser")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SendToUser([FromQuery] string userId, [FromBody] string message)
        {
            await _hubContext.Clients.Group(userId).SendAsync("ReceiveNotification", message);
            return Ok(new { status = "Notification sent to user", userId, message });
        }


        [HttpGet("paged")]
        [Authorize]
        public async Task<IActionResult> GetUserNotifications(
        int pageNumber = 1,
        int pageSize = 10)
        {
        
            var userIdClaim = User.Identity.Name;

            var userId = Guid.Parse(userIdClaim);

            var result = await _notificationService.GetNotificationsPagedAsync(userId, pageNumber, pageSize);

            return Ok(result);
        }
    }
}