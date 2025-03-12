using System.ComponentModel.DataAnnotations;

namespace Presentation.Models;

public class AddMemberModel
{
    public IFormFile ProfileImage { get; set; } = null!;

    [Display(Name = "First name", Prompt = "Your first name")]
    [Required(ErrorMessage = "Please enter a first name")]
    public string FirstName { get; set; } = null!;

    [Display(Name = "Last name", Prompt = "Your last name")]
    [Required(ErrorMessage = "Please enter a last name")]
    public string LastName { get; set; } = null!;


    [Display(Name = "Email", Prompt = "Your email address")]
    [Required(ErrorMessage = "Please enter an email address")]
    [DataType(DataType.EmailAddress)]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Please enter a valid email address")]
    public string Email { get; set; } = null!;


    [Display(Name = "Phone", Prompt = "Your phone number")]
    [Required(ErrorMessage = "Please enter a phone number")]
    [DataType(DataType.PhoneNumber)]
    public string Phone { get; set; } = null!;


    [Display(Name = "Job Title", Prompt = "Your job title")]
    [Required(ErrorMessage = "Please enter a job title")]
    public string JobTitle { get; set; } = null!;


    [Display(Name = "Address", Prompt = "Your address")]
    [Required(ErrorMessage = "Please enter an address")]
    public string Address { get; set; } = null!;

    [Display(Name = "Date of Birth")]
    [Required(ErrorMessage = "Please select date of birth")]
    public DateOfBirthModel DateOfBirth { get; set; } = new DateOfBirthModel();

}
