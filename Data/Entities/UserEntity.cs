using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Data.Entities;

public class UserEntity : IdentityUser
{
    [Column(TypeName = "nvarchar(max)")]
    public string? UserImageUrl { get; set; }

    [Required]
    [ProtectedPersonalData]
    [Column(TypeName = "nvarchar(50)")]
    public string FirstName { get; set; } = null!;

    [Required]
    [ProtectedPersonalData]
    [Column(TypeName = "nvarchar(50)")]
    public string LastName { get; set; } = null!;

    [ProtectedPersonalData]
    [Column(TypeName = "nvarchar(50)")]
    public string? JobTitle { get; set; }

    [ProtectedPersonalData]
    [Column(TypeName = "date")]
    public DateTime? DateOfBirth { get; set; }

    public int? AddressId { get; set; }
    [ForeignKey(nameof(AddressId))]
    public AddressEntity? Address { get; set; }

    public ICollection<ProjectUserEntity> ProjectUsers { get; set; } = new List<ProjectUserEntity>();

    public ICollection<NotificationDisMissedEntity> DismissedNotifications { get; set; } = new List<NotificationDisMissedEntity>();
}
