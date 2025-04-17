using System.ComponentModel.DataAnnotations;

namespace Business.Models.Users;

public class AddressEditModel
{
    public int Id { get; set; }


    [Display(Name = "Street name", Prompt = "Your street name")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    [StringLength(50, ErrorMessage = "Maximum 50 characters")]
    public string StreetName { get; set; } = null!;


    [Display(Name = "Street number", Prompt = "Your street number")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    [StringLength(5, ErrorMessage = "Maximum 5 characters")]
    public string StreetNumber { get; set; } = null!;


    [Display(Name = "Postal code", Prompt = "Your postal code")]
    [DataType(DataType.PostalCode)]
    [Required(ErrorMessage = "Required")]
    [StringLength(5, ErrorMessage = "Maximum 5 characters")]
    public string PostalCode { get; set; } = null!;


    [Display(Name = "City", Prompt = "Your city")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    [StringLength(50, ErrorMessage = "Maximum 50 characters")]
    public string City { get; set; } = null!;
}
