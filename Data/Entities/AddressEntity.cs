using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities;

//[Index(nameof(AddressId), IsUnique = true)]
public class AddressEntity
{
    [Key, ForeignKey("User")]
    public string UserId { get; set; } = null!;

    [Required]
    [Column(TypeName = "nvarchar(50)")]
    public string StreetName { get; set; } = null!;

    [Required]
    [Column(TypeName = "nvarchar(5)")]
    public string StreetNumber { get; set; } = null!;

    [Required]
    [Column(TypeName = "varchar(5)")]
    public string PostalCode { get; set; } = null!;

    [Required]
    [Column(TypeName = "nvarchar(50)")]
    public string City { get; set; } = null!;

    public UserEntity User { get; set; } = null!;
}
