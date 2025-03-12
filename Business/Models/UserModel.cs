namespace Business.Models;

public class UserModel
{
    public int UserId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;

    public RoleModel Role { get; set; } = null!;
    public AddressModel Address { get; set; } = null!;
    public DateOfBirthModel DateOfBirth { get; set; } = null!;
}
