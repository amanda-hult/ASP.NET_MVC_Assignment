using System.ComponentModel.DataAnnotations;

namespace Presentation.Models;

public class SignInModel
{
    [Display(Name = "Email", Prompt = "Your email address")]
    [Required(ErrorMessage = "Enter your email address")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;


    [Display(Name = "Password", Prompt = "Enter your password")]
    [Required(ErrorMessage = "Enter your password")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
}
