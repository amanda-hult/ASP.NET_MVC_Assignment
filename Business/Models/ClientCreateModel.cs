using Microsoft.AspNetCore.Http;

namespace Business.Models;

public class ClientCreateModel
{
    public IFormFile? ClientImage { get; set; }

    public string ClientName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Location { get; set; }

    public string? Phone { get; set; } 
}
