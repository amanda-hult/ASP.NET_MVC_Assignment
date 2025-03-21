using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities;

//[Index(nameof(Email), IsUnique = true)]
public class UserEntity : IdentityUser
{
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


    public AddressEntity? Address { get; set; } = null!;

    public int DateOfBirthId { get; set; }
    [ForeignKey("DateOfBirthId")]
    public DateOfBirthEntity DateOfBirth { get; set; } = null!;

}
