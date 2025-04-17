using Business.Models.Users;
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

    public static AddressEntity Update(AddressEditModel model)
    {
        return new AddressEntity
        {
            AddressId = model.Id,
            StreetName = model.StreetName,
            StreetNumber = model.StreetNumber,
            PostalCode = model.PostalCode,
            City = model.City,
        };
    }
}
