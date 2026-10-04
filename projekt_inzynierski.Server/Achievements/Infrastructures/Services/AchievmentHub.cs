namespace projekt_inzynierski.Server.Achievments.Infrastructures.Services
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;

    [Authorize]
    public class AchievementsHub : Hub
    {
        // The group is derived from the authenticated identity, never from client input,
        // so a user can only listen to their own achievements.
        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.Identity?.Name;
            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"user-{userId}");
            }

            await base.OnConnectedAsync();
        }
    }
}
