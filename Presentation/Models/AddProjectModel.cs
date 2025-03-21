using System.ComponentModel.DataAnnotations;

namespace Presentation.Models;

public class AddProjectModel
{
    [DataType(DataType.Upload)]
    public IFormFile? ProjectImage { get; set; }

    [Display(Name = "Project Name", Prompt = "Project Name")]
    [Required(ErrorMessage = "Required")]
    public string ProjectName { get; set; } = null!;


    [Display(Name = "Client Name", Prompt = "Client Name")]
    [Required(ErrorMessage = "Required")]
    public string ClientName { get; set; } = null!;


    [Display(Name = "Description", Prompt = "Type something")]
    [Required(ErrorMessage = "Required")]
    public string Description { get; set; } = null!;


    [Display(Name = "Start Date")]
    [Required(ErrorMessage = "Required")]
    public DateTime StartDate { get; set; }

    [Display(Name = "End Date")]
    [Required(ErrorMessage = "Required")]
    public DateTime EndDate { get; set; }



    [Display(Name = "Members")]
    [Required(ErrorMessage = "Please choose at least one member")]
    public string Member { get; set; } = null!;




    [Display(Name = "Budget")]
    [Required(ErrorMessage = "Required")]
    public string Budget { get; set; } = null!;

}
