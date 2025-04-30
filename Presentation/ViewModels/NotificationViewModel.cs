using Business.Models.Notifications;

namespace Presentation.ViewModels;

public class NotificationViewModel
{
    public IEnumerable<NotificationModel> Notifications { get; set; } = new List<NotificationModel>();
}
