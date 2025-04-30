using System.ComponentModel.DataAnnotations;

namespace Business.Models;

public class SignUpModel
{
    [Display(Name = "First name", Prompt = "Your first name")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "First name is required")]
    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [Display(Name = "Last name", Prompt = "Your last name")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Last name is required")]
    [StringLength(50)]
    public string LastName { get; set; } = null!;


    [Display(Name = "Email", Prompt = "Your email address")]
    [Required(ErrorMessage = "Email address is required")]
    [DataType(DataType.EmailAddress)]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Please enter a valid email address")]
    public string Email { get; set; } = null!;


    [Display(Name = "Password", Prompt = "Enter your password")]
    [Required(ErrorMessage = "You need to enter a password")]
    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*\d)(?=.*[a-zA-ZåäöÅÄÖ])[\w\W]{8,}$", ErrorMessage = "Your password is not strong enough")]
    public string Password { get; set; } = null!;


    [Display(Name = "Confirm password", Prompt = "Confirm your password")]
    [Required(ErrorMessage = "You need to confirm your password")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; } = null!;


    [Display(Name = "Terms & Conditions", Prompt = "I accept the terms & conditions")]
    [Required(ErrorMessage = "You need to accept the terms & conditions.")]
    public bool? TermsAndConditions { get; set; }
}
