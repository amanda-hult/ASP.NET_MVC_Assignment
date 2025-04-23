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

    public static AddressEntity Create(AddressEditModel model)
    {
        return new AddressEntity
        {
            StreetName = model.StreetName,
            StreetNumber = model.StreetNumber,
            PostalCode = model.PostalCode,
            City = model.City,
        };
    }

    public static void Update(AddressEditModel model, AddressEntity entity)
    {
        entity.AddressId = model.Id;
        entity.StreetName = model.StreetName;
        entity.StreetNumber = model.StreetNumber;
        entity.PostalCode = model.PostalCode;
        entity.City = model.City;
    }
}
