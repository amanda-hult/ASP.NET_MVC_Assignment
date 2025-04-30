using System.Security.Claims;
using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Presentation.ViewModels;

namespace Presentation.ViewComponents;

public class NotificationViewComponent : ViewComponent
{
    private readonly INotificationService _notificationService;

    public NotificationViewComponent(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var userId = Request.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "anonymous";
        var isAdmin = Request.HttpContext.User.IsInRole("Admin");
        //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "anonymous";
        //if (string.IsNullOrEmpty(userId))
        //    return Unauthorized();

        var notifications = await _notificationService.GetNotificationsAsync(isAdmin, userId);

        var viewModel = new NotificationViewModel
        {
            Notifications = notifications
        };

        return View(viewModel);
    }
}
