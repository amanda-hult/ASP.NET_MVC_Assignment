using Business.Models.Notifications;

namespace Presentation.ViewModels;

public class NotificationViewModel
{
    //public NotificationCreateModel CreateModel { get; set; } = new NotificationCreateModel();
    public IEnumerable<NotificationModel> Notifications { get; set; } = new List<NotificationModel>();
}
