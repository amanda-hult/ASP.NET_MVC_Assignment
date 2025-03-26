using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class AddressEntity
{
    [Key]
    public int AddressId { get; set; }

    [Column(TypeName = "nvarchar(50)")]
    public string? StreetName { get; set; }

    [Column(TypeName = "nvarchar(5)")]
    public string? StreetNumber { get; set; }

    [Column(TypeName = "varchar(5)")]
    public string? PostalCode { get; set; }

    [Column(TypeName = "nvarchar(50)")]
    public string? City { get; set; }

    public ICollection<UserEntity> Users { get; set; } = new List<UserEntity>();
}
