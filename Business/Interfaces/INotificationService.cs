using Business.Models.Notifications;
using Data.Entities;

namespace Business.Interfaces;

public interface INotificationService
{
    Task AddNotificationAsync(NotificationCreateModel notification, string userId = "anonymous");
    Task<IEnumerable<NotificationModel>> GetNotificationsAsync(string userId, int take = 5);
    Task DisMissNotificationAsync(string notificationId, string userId);
}