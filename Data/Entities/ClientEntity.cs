using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities;

[Index(nameof(ClientName), IsUnique = true)]
public class ClientEntity
{
    [Key]
    public int ClientId { get; set; }

    [Column(TypeName = "nvarchar(max)")]
    public string? ClientImageUrl { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(50)")]
    public string ClientName { get; set; } = null!;

    [Required]
    [Column(TypeName = "nvarchar(150)")]
    public string Email { get; set; } = null!;

    [Column(TypeName = "nvarchar(50)")]
    public string? Location { get; set; }

    [MaxLength(20)]
    [Column(TypeName = "nvarchar(20)")]
    public string? Phone { get; set; }

    [Column(TypeName = "date")]
    public DateTime Created { get; set; } = DateTime.Now;

    public ICollection<ProjectEntity> Projects { get; set; } = [];

}
