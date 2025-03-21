using System.ComponentModel.DataAnnotations;
using Business.Models;

namespace Presentation.Models;

public class AddMemberModel
{
    [DataType(DataType.Upload)]
    public IFormFile? ProfileImage { get; set; }


    [Display(Name = "First name", Prompt = "Your first name")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public string FirstName { get; set; } = null!;


    [Display(Name = "Last name", Prompt = "Your last name")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public string LastName { get; set; } = null!;



    [Display(Name = "Email", Prompt = "Your email address")]
    [Required(ErrorMessage = "Required")]
    [DataType(DataType.EmailAddress)]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email")]
    public string Email { get; set; } = null!;



    [Display(Name = "Phone", Prompt = "Your phone number")]
    [DataType(DataType.PhoneNumber)]
    public string? Phone { get; set; }



    [Display(Name = "Job Title", Prompt = "Your job title")]
    [Required(ErrorMessage = "Required")]
    public string JobTitle { get; set; } = null!;



    [Display(Name = "Address", Prompt = "Your address")]
    [Required(ErrorMessage = "Required")]
    public string Address { get; set; } = null!;



    [Display(Name = "Date of Birth")]
    //[Required(ErrorMessage = "Please select date of birth")]
    public DateOfBirthCreateModel DateOfBirth { get; set; } = new DateOfBirthCreateModel();

}
