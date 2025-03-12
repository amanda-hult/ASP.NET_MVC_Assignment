using Business.Models;
using Data.Entities;

namespace Business.Factories;

public static class UserFactory
{
    public static UserModel Create(UserEntity entity)
    {
        return new UserModel
        {
            UserId = entity.UserId,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email = entity.Email,
            Phone = entity.Phone,
            Role = new RoleModel 
            {   RoleId = entity.Role.RoleId,
                Title = entity.Role.Title,            
            },
            Address = new AddressModel
            {
                AddressId = entity.Address.AddressId,
                StreetName = entity.Address.StreetName,
                StreetNumber = entity.Address.StreetNumber,
                PostalCode = entity.Address.PostalCode,
                City = entity.Address.City,
            },
            DateOfBirth = new DateOfBirthModel
            {
                DateOfBirthId = entity.DateOfBirth.DateOfBirthId,
                Date = entity.DateOfBirth.Date,
                Month = entity.DateOfBirth.Month,
                Year = entity.DateOfBirth.Year,
            },
        };
    }

    //public static UserModel Create(UserEntity entity)
    //{
    //    return new UserModel
    //    {
    //        UserId = entity.UserId,
    //        FirstName = entity.FirstName,
    //        LastName = entity.LastName,
    //        Email = entity.Email,
    //        Phone = entity.Phone,
    //        Role = new RoleModel
    //        {
    //            RoleId = entity.Role.RoleId,
    //            Title = entity.Role.Title,
    //        },
    //        Address = new AddressModel
    //        {
    //            AddressId = entity.Address.AddressId,
    //            StreetName = entity.Address.StreetName,
    //            StreetNumber = entity.Address.StreetNumber,
    //            PostalCode = entity.Address.PostalCode,
    //            City = entity.Address.City,
    //        },
    //        DateOfBirth = new DateOfBirthModel
    //        {
    //            DateOfBirthId = entity.DateOfBirth.DateOfBirthId,
    //            Date = entity.DateOfBirth.Date,
    //            Month = entity.DateOfBirth.Month,
    //            Year = entity.DateOfBirth.Year,
    //        },
    //    };
    //}
    //public static UserEntity Create(UserCreateModel model)
    //{
    //    return new UserEntity
    //    {
    //        FirstName = model.FirstName,
    //        LastName = model.LastName,
    //        Email = model.Email,
    //        Phone = model.Phone,
    //        RoleId = 
    //        Address = 
    //        DateOfBirth = 
    //    };
    //}
}
