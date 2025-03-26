using System.ComponentModel.DataAnnotations;

namespace Business.Models;

public class UserCreateModel
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string? JobTitle { get; set; }

    public AddressCreateModel? Address { get; set; }
    public string Password { get; set; } = null!;
    public DateTime? DateOfBirth { get; set; }
}
