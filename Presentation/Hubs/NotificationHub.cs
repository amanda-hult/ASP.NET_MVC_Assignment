using Microsoft.AspNetCore.SignalR;

namespace Presentation.Hubs;

public class NotificationHub : Hub
{
    public async Task SendNotificationToAll(object notification)
    {
        await Clients.All.SendAsync("RecieveNotification", notification);
    }

    public async Task SendNotificationToAdmins(object notification)
    {
        await Clients.Group("Admins").SendAsync("AdminRecieveNotification", notification);
    }

    public override async Task OnConnectedAsync()
    {
        var admin = Context.User.IsInRole("Admin");
        if (admin)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "Admins");
        }
        await base.OnConnectedAsync();
    }
}
