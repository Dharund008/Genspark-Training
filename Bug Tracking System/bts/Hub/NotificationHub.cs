using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Bts.Hubs
{
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();

            //var userId = httpContext?.Request.Query["userId"].ToString();
            var role = httpContext?.Request.Query["role"].ToString();

            // if (!string.IsNullOrEmpty(userId))
            //     await Groups.AddToGroupAsync(Context.ConnectionId, userId);

            if (!string.IsNullOrEmpty(role))
                await Groups.AddToGroupAsync(Context.ConnectionId, role.ToUpper());

            await base.OnConnectedAsync();
        }

        // public async Task SendToUser(string userId, string message)
        // {
        //     await Clients.Group(userId).SendAsync("ReceiveMessage", message);
        // }

        public async Task SendToRole(string role, string message)
        {
            await Clients.Group(role.ToUpper()).SendAsync("ReceiveMessage", message);
        }
    }
}
