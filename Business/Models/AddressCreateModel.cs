using System.ComponentModel.DataAnnotations;

namespace Business.Models;

public class AddressCreateModel
{
    [Required]
    public string StreetName { get; set; } = null!;

    [Required]
    public string StreetNumber { get; set; } = null!;

    [Required]
    public string PostalCode { get; set; } = null!;

    [Required]
    public string City { get; set; } = null!;
}
