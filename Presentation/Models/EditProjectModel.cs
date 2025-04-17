using Business.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    public DateTime? EndDate { get; set; }


    [Display(Name = "Budget")]
    public decimal? Budget { get; set; }



    [Display(Name = "Client Name", Prompt = "Client Name")]
    [Required(ErrorMessage = "Required")]
    public int? SelectedClientId { get; set; } = null!;

    public IEnumerable<SelectListItem> Clients { get; set; } = new List<SelectListItem>();



    [Display(Name = "Members")]
    [Required(ErrorMessage = "Please choose at least one member")]
    public string? SelectedMemberIds { get; set; } = null!;

    //public IEnumerable<SelectListItem> Members { get; set; } = new List<SelectListItem>();



    [Display(Name = "Status")]
    [Required(ErrorMessage = "Please select a status")]
    public int? SelectedStatusId { get; set; }

    public IEnumerable<SelectListItem> Statuses { get; set; } = new List<SelectListItem>();
}
