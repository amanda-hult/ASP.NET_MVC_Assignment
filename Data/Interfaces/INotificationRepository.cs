using Data.Entities;

namespace Data.Interfaces;

public interface INotificationRepository : IBaseRepository<NotificationEntity>
{
    Task<IEnumerable<NotificationEntity>> GetNotificationsForUsersAsync();
}