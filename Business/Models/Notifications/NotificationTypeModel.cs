namespace Business.Models.Notifications;

public class NotificationTypeModel
{
    public int NotificationTypeId { get; set; }
    public string NotificationType { get; set; } = null!;
    public List<NotificationCreateModel> Notifications { get; set; } = new List<NotificationCreateModel>();
}
