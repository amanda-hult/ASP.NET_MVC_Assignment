namespace Business.Models.Notifications;

public class NotificationCreateModel
{
    public int TargetGroupId { get; set; } = 1;
    public NotificationTargetGroupModel NotificationTargetGroup { get; set; } = null!;


    public int NotificationTypeId { get; set; }
    public NotificationTypeModel NotificationType { get; set; } = null!;

    public string Image { get; set; } = null!;

    public string Message { get; set; } = null!;

    public DateTime Created { get; set; } = DateTime.UtcNow;
    public List<NotificationDismissedModel> DismissedNotifications { get; set; } = new List<NotificationDismissedModel>();
}
