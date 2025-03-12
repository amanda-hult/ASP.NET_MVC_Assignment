using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities;

[Index(nameof(ClientName), IsUnique = true)]
public class ClientEntity
{
    [Key]
    public int ClientId { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(50)")]
    public string ClientName { get; set; } = null!;
    public ICollection<ProjectEntity> Projects { get; set; } = [];
}
