namespace Business.Models;

public class AddressModel
{
    public int AddressId { get; set; }
    public string StreetName { get; set; } = null!;
    public string StreetNumber { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public string City { get; set; } = null!;
    public List<UserModel> Users { get; set; } = new List<UserModel>();
}
