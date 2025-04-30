using Business.Interfaces;
using Business.Models.Notifications;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Presentation.Hubs;
using System.Security.Claims;

namespace Presentation.Services;


public class HelperService(INotificationService notificationService, IHubContext<NotificationHub> hubContext, IProjectService projectService, IClientService clientService, UserManager<UserEntity> userManager, IHttpContextAccessor httpContextAccessor) : IHelperService
{
    private readonly INotificationService _notificationService = notificationService;
    private readonly IHubContext<NotificationHub> _hubContext = hubContext;
    private readonly IProjectService _projectService = projectService;
    private readonly IClientService _clientService = clientService;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly UserManager<UserEntity> _userManager = userManager;

    public async Task HandleNotifications(string? id, int notificationTypeId, string imageUri, string action, bool showAll)
    {
        switch (notificationTypeId)
        {
            case 1:
                var member = await _userManager.FindByIdAsync(id);
                if (member != null)
                {
                    NotificationCreateModel notificationCreateModel;

                    if (action == "SignIn")
                    {
                        notificationCreateModel = new NotificationCreateModel
                        {
                            Message = $"{member.FirstName} {member.LastName} signed in.",
                            NotificationTypeId = notificationTypeId,
                            TargetGroupId = 2,
                            Image = imageUri ?? "/images/avatar-standard.svg"
                        };
                    }
                    else
                    {
                        notificationCreateModel = new NotificationCreateModel
                        {
                            Message = action == "Add"
                            ? $"New member: {member.FirstName} {member.LastName} was added."
                            : $"Member {member.FirstName} {member.LastName} was updated.",
                            NotificationTypeId = notificationTypeId,
                            TargetGroupId = 1,
                            Image = imageUri ?? "/images/avatar-standard.svg"
                        };
                    }
                    await _notificationService.AddNotificationAsync(notificationCreateModel);
                }
                break;

            case 2:
                var project = await _projectService.GetProjectAsync(int.Parse(id));
                if (project != null)
                {
                    var notificationCreateModel = new NotificationCreateModel
                    {
                        Message = action == "Add"
                        ? $"{project.ProjectName} was added."
                        : $"{project.ProjectName} was updated.",
                        NotificationTypeId = notificationTypeId,
                        TargetGroupId = 1,
                        Image = imageUri ?? "/images/project-image-standard.svg"
                    };
                    await _notificationService.AddNotificationAsync(notificationCreateModel);
                }
                break;

            case 3:
                var client = await _clientService.GetClientAsync(int.Parse(id));
                if (client != null)
                {
                    var notificationCreateModel = new NotificationCreateModel
                    {
                        Message = action == "Add"
                        ? $"{client.ClientName} was added."
                        : $"{client.ClientName} was updated.",
                        NotificationTypeId = notificationTypeId,
                        TargetGroupId = 1,
                        Image = imageUri ?? "/images/client-avatar-standard.svg"
                    };
                    await _notificationService.AddNotificationAsync(notificationCreateModel);
                }
                break;

            default:
                break;
        }

        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        IEnumerable<NotificationModel> notifications;
        if (showAll)
        {
            notifications = await _notificationService.GetNotificationsAsync(false, userId);
        }
        else
        {
            notifications = await _notificationService.GetNotificationsAsync(true, userId);
        }

        var newNotification = notifications.OrderByDescending(x => x.Created).FirstOrDefault();
        if (newNotification != null)
        {
            if (!showAll)
            {
                await _hubContext.Clients.Group("Admins").SendAsync("AdminRecieveNotification", newNotification);
            }
            else
            {
                await _hubContext.Clients.All.SendAsync("RecieveNotification", newNotification);
            }
        }
    }
}
