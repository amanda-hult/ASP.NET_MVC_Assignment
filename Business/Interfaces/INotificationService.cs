using Business.Models.Notifications;

namespace Business.Interfaces;

public interface INotificationService
{
    Task AddNotificationAsync(NotificationCreateModel notification, string userId = "anonymous");
    Task<IEnumerable<NotificationModel>> GetNotificationsAsync(bool isAdmin, string userId, int take = 5);
    Task DisMissNotificationAsync(string notificationId, string userId);
}