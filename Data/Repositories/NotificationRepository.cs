using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class NotificationRepository(DataContext context) : BaseRepository<NotificationEntity>(context), INotificationRepository
{
    public async Task<IEnumerable<NotificationEntity>> GetNotificationsForUsersAsync()
    {
        return await _dbSet.Where(x => x.TargetGroupId == 1).Include(x => x.NotificationTargetGroup).Include(x => x.DismissedNotifications).ToListAsync();
    }
}

