namespace Business.Models.Notifications;

public class NotificationTargetGroupModel
{
    public int NotificationTargetGroupId { get; set; }
    public string TargetGroup { get; set; } = null!;
    public List<NotificationCreateModel> Notifications { get; set; } = new List<NotificationCreateModel>();
}
