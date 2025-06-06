using Microsoft.AspNetCore.SignalR;

namespace NotifyAPI.Misc
{
    public class NotificationHub : Hub
    {
        // Optionally log connected users or send welcome
        public override Task OnConnectedAsync()
        {
            Console.WriteLine($"Client connected: {Context.ConnectionId}");
            return base.OnConnectedAsync();
        }
    }
}
