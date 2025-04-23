using System.ComponentModel.DataAnnotations;

namespace Presentation.Models
{
    public class AddMemberToProjectModel
    {
        public int Id { get; set; }

        [Display(Name = "Project Name", Prompt = "Project Name")]
        [Required(ErrorMessage = "Required")]
        public string ProjectName { get; set; } = null!;

        [Display(Name = "Members")]
        public string? SelectedMemberIdsUpdateMembers { get; set; } = null!;

    }
}
