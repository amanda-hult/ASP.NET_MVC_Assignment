using System.ComponentModel.DataAnnotations;
using Business.Models.Users;

namespace Presentation.Models;

public class AddMemberModel
{
    [DataType(DataType.Upload)]
    public IFormFile? ProfileImage { get; set; }


    [Display(Name = "First name", Prompt = "Your first name")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    [StringLength(50)]
    public string FirstName { get; set; } = null!;


    [Display(Name = "Last name", Prompt = "Your last name")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    [StringLength(50)]
    public string LastName { get; set; } = null!;


    [Display(Name = "Email", Prompt = "Your email address")]
    [Required(ErrorMessage = "Required")]
    [DataType(DataType.EmailAddress)]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email")]
    public string Email { get; set; } = null!;


    [Display(Name = "Phone", Prompt = "Your phone number")]
    [DataType(DataType.PhoneNumber)]
    [StringLength(20)]
    public string? Phone { get; set; }


    [Display(Name = "Job Title", Prompt = "Your job title")]
    [StringLength(50)]
    public string? JobTitle { get; set; }


    [Display(Name = "Address", Prompt = "Your address")]
    public AddressCreateModel? Address { get; set; }


    [Display(Name = "Password", Prompt = "Temporary password")]
    [Required(ErrorMessage = "Required")]
    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9])[\s\S]{8,}$", ErrorMessage = "Your password is not strong enough")]
    public string Password { get; set; } = null!;


    [Required(ErrorMessage = "Required")]
    public int? SelectedDay { get; set; }


    [Required(ErrorMessage = "Required")]
    public int? SelectedMonth { get; set; }


    [Required(ErrorMessage = "Required")]
    public int? SelectedYear { get; set; }

    public IEnumerable<int> Days { get; set; } = Enumerable.Range(1, 31).ToList();
    public IEnumerable<int> Months { get; set; } = Enumerable.Range(1, 12).ToList();
    public IEnumerable<int> Years { get; set; } = Enumerable.Range(DateTime.Now.Year - 100, 100 - 18 + 1).Reverse().ToList();

}
