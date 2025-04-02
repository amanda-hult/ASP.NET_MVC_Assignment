using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class NotificationTargetGroupEntity
{
    [Key]
    public int NotificationTargetGroupId { get; set; }

    [Column(TypeName = "nvarchar(50)")]
    public string TargetGroup { get; set; } = null!;
    public ICollection<NotificationEntity> Notifications { get; set; } = new List<NotificationEntity>();
}
