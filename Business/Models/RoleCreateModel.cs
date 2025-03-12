using System.ComponentModel.DataAnnotations;

namespace Business.Models;

public class RoleCreateModel
{
    [Required]
    public string Title { get; set; } = null!;
}
