using System.Security.Claims;
using Business.Interfaces;
using Business.Models.Notifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Presentation.Hubs;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotificationController : ControllerBase
{
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly INotificationService _notificationService;

    public NotificationController(IHubContext<NotificationHub> hubContext, INotificationService notificationService)
    {
        _hubContext = hubContext;
        _notificationService = notificationService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateNotification(NotificationCreateModel model)
    {
        await _notificationService.AddNotificationAsync(model);
        var notifications = await _notificationService.GetNotificationsAsync("anonymous");
        var newNotification = notifications.OrderByDescending(x => x.Created).FirstOrDefault();
        if (newNotification != null)
        {
            await _hubContext.Clients.All.SendAsync("RecieveNotification", newNotification);
        }
        return Ok(new { success = true });
    }

    [HttpGet]
    public async Task<IActionResult> GetNotifications()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "anonymous";
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var notifications = await _notificationService.GetNotificationsAsync(userId);

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
