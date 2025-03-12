using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities;

[Index(nameof(Email), IsUnique = true)]
public class UserEntity
{
    [Key]
    public int UserId { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(50)")]
    public string FirstName { get; set; } = null!;

    [Required]
    [Column(TypeName = "nvarchar(50)")]
    public string LastName { get; set; } = null!;

    [Required]
    [Column(TypeName = "nvarchar(150)")]
    public string Email { get; set; } = null!;

    [Required]
    [Column(TypeName = "varchar(20)")]
    public string Phone { get; set; } = null!;


    public int RoleId { get; set; }
    [ForeignKey("RoleId")]
    public RoleEntity Role { get; set; } = null!;

    public int AddressId { get; set; }
    [ForeignKey("AddressId")]
    public AddressEntity Address { get; set; } = null!;

    public int DateOfBirthId { get; set; }
    [ForeignKey("DateOfBirthId")]
    public DateOfBirthEntity DateOfBirth { get; set; } = null!;

}
