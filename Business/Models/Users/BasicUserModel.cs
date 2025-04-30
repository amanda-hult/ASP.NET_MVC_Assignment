namespace Business.Models.Users;

public class BasicUserModel
{
    public string Id { get; set; } = null!;
    public string? UserImageUrl { get; set; }
    public string? FullName { get; set; } = null!;
}
