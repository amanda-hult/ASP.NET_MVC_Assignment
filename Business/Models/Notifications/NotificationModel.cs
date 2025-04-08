namespace Business.Models.Notifications;

public class NotificationModel
{
    public string NotificationId { get; set; } = null!;
    public int TargetGroupId { get; set; } = 1;
    public NotificationTargetGroupModel NotificationTargetGroup { get; set; } = null!;


    public int NotificationTypeId { get; set; }
    public NotificationTypeModel NotificationType { get; set; } = null!;

    public string Image { get; set; } = null!;

    public string Message { get; set; } = null!;

    public DateTimeOffset Created { get; set; } = DateTimeOffset.UtcNow;
    public List<NotificationDismissedModel> DismissedNotifications { get; set; } = new List<NotificationDismissedModel>();
}
