using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities;

[Index(nameof(ClientName), IsUnique = true)]
public class ClientEntity
{
    [Key]
    public int ClientId { get; set; }


    //[DataType(DataType.Upload)]
    //public IFormFile? ClientImage { get; set; }


    [Required]
    [Column(TypeName = "nvarchar(50)")]
    public string ClientName { get; set; } = null!;


    [Required]
    [Column(TypeName = "nvarchar(150)")]
    public string Email { get; set; } = null!;


    [Column(TypeName = "nvarchar(50)")]
    public string? Location { get; set; }


    [Column(TypeName = "varchar(17)")]
    public string? Phone { get; set; }

    [Column(TypeName = "date")]
    public DateTime Created { get; set; } = DateTime.Now;


    // koppla till om klienten ingår i ett projekt
    [Column(TypeName = "varchar(8)")]
    public string Status { get; set; } = null!;

    public ICollection<ProjectEntity> Projects { get; set; } = [];

}
