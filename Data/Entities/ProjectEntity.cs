using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities;

[Index(nameof(Description), nameof(ClientId), IsUnique = true)]
public class ProjectEntity
{
    [Key]
    public int ProjectId { get; set; }

    //[DataType(DataType.Upload)]
    //public IFormFile? ProjectImage { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(50)")]
    public string ProjectName { get; set; } = null!;

    [Required]
    [Column(TypeName = "nvarchar(max)")]
    public string Description { get; set; } = null!;

    [Required]
    [Column(TypeName = "date")]
    public DateTime StartDate { get; set; }

    [Column(TypeName = "date")]
    public DateTime? EndDate { get; set; }

    [Column(TypeName ="decimal(18,2)")]
    public string? Budget { get; set; }

    public int ClientId { get; set; }
    [ForeignKey("ClientId")]
    public ClientEntity Client { get; set; } = null!;

    public string UserId { get; set; } = null!;
    [ForeignKey("UserId")]
    public UserEntity User { get; set; } = null!;

    public int StatusId { get; set; }
    [ForeignKey("StatusId")]
    public StatusEntity Status { get; set; } = null!;

    public ICollection<ProjectUserEntity> ProjectUsers { get; set; } = new List<ProjectUserEntity>();
}
