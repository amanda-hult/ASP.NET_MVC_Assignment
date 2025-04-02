using Microsoft.AspNetCore.Http;

namespace Business.Models.Clients;

public class ClientModel
{
    public int Id { get; set; }
    public IFormFile? ClientImage { get; set; }
    public string ClientName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Location { get; set; }
    public string? Phone { get; set; }
    public DateTime Created { get; set; }
    public string Status { get; set; } = null!;
}
