namespace Business.Models.Users;

public class UserEditModel
{
    public string Id { get; set; } = null!;
    public string? ProfileImage { get; set; }
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string? JobTitle { get; set; }

    public AddressEditModel? Address { get; set; }
    //public string? Password { get; set; }
    public DateTime? DateOfBirth { get; set; }
}
