using System.ComponentModel.DataAnnotations;

namespace Presentation.Models;

public class AddProjectModel
{
    public IFormFile ProjectImage { get; set; } = null!;

    [Display(Name = "Project Name", Prompt = "Project Name")]
    [Required(ErrorMessage = "Please enter a project name")]
    public string ProjectName { get; set; } = null!;

    [Display(Name = "Client Name", Prompt = "Client Name")]
    [Required(ErrorMessage = "Please enter a client name")]
    public string ClientName { get; set; } = null!;


    [Display(Name = "Description", Prompt = "Type something")]
    [Required(ErrorMessage = "Please enter a description")]
    public string Description { get; set; } = null!;


    [Display(Name = "Start Date")]
    [Required(ErrorMessage = "Please enter a start date")]
    public DateTime StartDate { get; set; }

    [Display(Name = "End Date")]
    [Required(ErrorMessage = "Please enter a end date")]
    public DateTime EndDate { get; set; }



    [Display(Name = "Members")]
    [Required(ErrorMessage = "Please choose at least one member")]
    public string Member { get; set; } = null!;




    [Display(Name = "Budget")]
    [Required(ErrorMessage = "Please enter a budget")]
    public string Budget { get; set; } = null!;

}
