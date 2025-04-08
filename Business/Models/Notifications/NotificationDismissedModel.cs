using Business.Models.Users;

namespace Business.Models.Notifications;

public class NotificationDismissedModel
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;
    public UserModel User { get; set; } = null!;


    public string NotificationId { get; set; } = null!;
    public NotificationCreateModel Notification { get; set; } = null!;
}
