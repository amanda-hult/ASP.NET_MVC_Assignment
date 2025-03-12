using Business.Models;
using Data.Entities;

namespace Business.Factories;

public static class AddressFactory
{
    public static AddressEntity Create(AddressCreateModel model)
    {
        return new AddressEntity
        {
            StreetName = model.StreetName,
            StreetNumber = model.StreetNumber,
            PostalCode = model.PostalCode,
            City = model.City,
        };
    }
}
