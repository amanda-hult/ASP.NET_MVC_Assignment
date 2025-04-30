using Business.Factories;
using Business.Interfaces;
using Business.Models.Notifications;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Business.Services;

public class NotificationService(INotificationRepository notificationRepository, INotificationDismissedRepository notificationDismissedRepository) : INotificationService
{
    private readonly INotificationRepository _notificationRepository = notificationRepository;
    private readonly INotificationDismissedRepository _notificationDismissedRepository = notificationDismissedRepository;

    public async Task AddNotificationAsync(NotificationCreateModel notification, string userId = "anonymous")
    {
        var notificationEntity = NotificationFactory.Create(notification);

        await _notificationRepository.CreateAsync(notificationEntity);
        await _notificationRepository.SaveAsync();
    }

    public async Task<IEnumerable<NotificationModel>> GetNotificationsAsync(bool isAdmin, string userId, int take = 5)
    {
        IEnumerable<NotificationEntity> notifications;

        if (isAdmin)
        {
            notifications = await _notificationRepository.GetAllAsync(query =>
                query.Include(x => x.DismissedNotifications)
            );
        }
        else
        {
            notifications = await _notificationRepository.GetNotificationsForUsersAsync();
        }

        var filteredNotifications = notifications.Where(x => !x.DismissedNotifications.Any(x => x.UserId == userId))
            .OrderByDescending(x => x.Created)
            .Take(take);

        var list = filteredNotifications.Select(NotificationFactory.Create).ToList();

        return list;
    }

    public async Task DisMissNotificationAsync(string notificationId, string userId)
    {

        // check if notification alredy is dismissed
        var alreadyDismissed = await _notificationDismissedRepository.ExistsAsync(x => x.UserId == userId && x.NotificationId == notificationId);
        if (alreadyDismissed)
            return;

        // if not => create dismissEntity
        var dismissEntity = new NotificationDisMissedEntity
        {
            NotificationId = notificationId,
            UserId = userId,
        };

        // add and save
        await _notificationDismissedRepository.CreateAsync(dismissEntity);
        await _notificationDismissedRepository.SaveAsync();
    }
}
