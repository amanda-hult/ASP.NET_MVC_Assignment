using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class NotificationEntity
{
    [Key]
    public string NotificationId { get; set; } = Guid.NewGuid().ToString();

    [ForeignKey(nameof(NotificationTargetGroup))]
    public int TargetGroupId { get; set; } = 1;
    public NotificationTargetGroupEntity NotificationTargetGroup { get; set; } = null!;

    [ForeignKey(nameof(NotificationType))]
    public int NotificationTypeId { get; set; }
    public NotificationTypeEntity NotificationType { get; set; } = null!;

    [Column(TypeName = "nvarchar(max)")]
    public string Image { get; set; } = null!;

    [Column(TypeName = "nvarchar(max)")]
    public string Message { get; set; } = null!;

    [Column(TypeName = "datetimeoffset")]
    public DateTimeOffset Created { get; set; } = DateTimeOffset.UtcNow;
    public ICollection<NotificationDisMissedEntity> DismissedNotifications { get; set; } = new List<NotificationDisMissedEntity>();
}
