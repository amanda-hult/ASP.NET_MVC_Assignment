using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class ProjectUserEntity
{
    public int ProjectId { get; set; }
    [ForeignKey("ProjectId")]
    public ProjectEntity Project { get; set; } = null!;

    public string UserId { get; set; } = null!;
    [ForeignKey("UserId")]
    public UserEntity User { get; set; } = null!;
}
