namespace Business.Models.Clients;

public class ClientCreateModel
{
    public string? ClientImage { get; set; }
    public string ClientName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Location { get; set; }
    public string? Phone { get; set; }
}
