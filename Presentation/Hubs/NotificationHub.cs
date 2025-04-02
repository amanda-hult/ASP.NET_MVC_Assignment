﻿using Microsoft.AspNetCore.SignalR;

namespace Presentation.Hubs;

public class NotificationHub : Hub
{
    public async Task SendNotificationToAll(object notification)
    {
        await Clients.All.SendAsync("RecieveNotification", notification);
    }

    //public async Task SendNotificationToAdmins(object notification)
    //{
    //    await Clients.All.SendAsync("AdminRecieveNotification", notification);
    //}
}
