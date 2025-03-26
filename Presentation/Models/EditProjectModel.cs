using Business.Models;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Models;

public class EditProjectModel
{
    public int Id { get; set; }

    [DataType(DataType.Upload)]
    public IFormFile? ProjectImage { get; set; }


    [Display(Name = "Project Name", Prompt = "Project Name")]
    [Required(ErrorMessage = "Required")]
    public string ProjectName { get; set; } = null!;


    [Display(Name = "Description", Prompt = "Type something")]
    [Required(ErrorMessage = "Required")]
    public string Description { get; set; } = null!;


    [Display(Name = "Start Date")]
    [Required(ErrorMessage = "Required")]
    public DateTime StartDate { get; set; }



    [Display(Name = "End Date")]
    [Required(ErrorMessage = "Required")]
    public DateTime EndDate { get; set; }


    [Display(Name = "Budget", Prompt = "0")]
    public string? Budget { get; set; }



    [Display(Name = "Client Name", Prompt = "Client Name")]
    [Required(ErrorMessage = "Required")]
    public ClientModel ClientName { get; set; } = null!;


    [Display(Name = "Members")]
    [Required(ErrorMessage = "Please choose at least one member")]
    public UserModel Member { get; set; } = null!;


    [Display(Name = "Status")]
    [Required(ErrorMessage = "Please select a status")]
    public StatusModel Status { get; set; } = null!;
}
