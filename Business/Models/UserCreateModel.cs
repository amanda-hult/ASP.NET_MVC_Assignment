using System.ComponentModel.DataAnnotations;

namespace Business.Models;

public class UserCreateModel
{
    [Required(ErrorMessage = "First name is required")]
    [Display(Name = "Your first name")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Last name is required")]
    [Display(Name = "Your last name")]
    public string LastName { get; set; } = null!;

    [Required(ErrorMessage = "Email address is required")]
    [Display(Name = "Your email address")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Phone number is required")]
    [Display(Name = "Your phone number")]
    public string Phone { get; set; } = null!;


    public int RoleId { get; set; }
    //public RoleCreateModel Role { get; set; } = null!;

    public int AddressId { get; set; }
    public AddressCreateModel Address { get; set; } = null!;

    public int DateOfBirthId { get; set; }
    public DateOfBirthCreateModel DateOfBirth { get; set; } = null!;
}
