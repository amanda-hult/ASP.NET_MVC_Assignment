using System.Security.Claims;
using Business.Interfaces;
using Business.Models.Notifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Presentation.Hubs;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotificationController(IHubContext<NotificationHub> hubContext, INotificationService notificationService) : ControllerBase
{
    private readonly IHubContext<NotificationHub> _hubContext = hubContext;
    private readonly INotificationService _notificationService = notificationService;

    [HttpPost]
    public async Task<IActionResult> CreateNotification(NotificationCreateModel model)
    {
        await _notificationService.AddNotificationAsync(model);
        var isAdmin = User.IsInRole("Admin");
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "anonymous";
        var notifications = await _notificationService.GetNotificationsAsync(isAdmin, userId);
        var newNotification = notifications.OrderByDescending(x => x.Created).FirstOrDefault();
        if (newNotification != null)
        {
            if (model.TargetGroupId == 2)
            {
                await _hubContext.Clients.All.SendAsync("AdminRecieveNotification", newNotification);
            }
            else
            {
                await _hubContext.Clients.All.SendAsync("RecieveNotification", newNotification);
            }
        }

        return Ok(new { success = true });
    }

    [HttpGet]
    public async Task<IActionResult> GetNotifications()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "anonymous";
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var isAdmin = User.IsInRole("Admin");
        var notifications = await _notificationService.GetNotificationsAsync(isAdmin, userId);

        return Ok(notifications);
    }

    [HttpPost("dismiss/{id}")]
    public async Task<IActionResult> DismissNotification(string id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "anonymous";
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        await _notificationService.DisMissNotificationAsync(id, userId);
        await _hubContext.Clients.User(userId).SendAsync("NotificationDismissed", id);
        return Ok(new { success = true });
    }
}
