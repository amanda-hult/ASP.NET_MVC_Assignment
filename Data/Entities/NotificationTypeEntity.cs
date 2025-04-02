using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class NotificationTypeEntity
{
    [Key]
    public int NotificationTypeId { get; set; }

    [Column(TypeName = "nvarchar(50)")]
    public string NotificationType { get; set; } = null!;
    public ICollection<NotificationEntity> Notifications { get; set; } = new List<NotificationEntity>();
}