using Business.Models;
using Business.Models.Users;
using Data.Entities;

namespace Business.Factories;

public static class UserFactory
{
    public static BasicUserModel CreateBasicUser(UserEntity entity)
    {
        return new BasicUserModel
        {
            Id = entity.Id,
            UserImageUrl = entity.UserImageUrl,
            FullName = $"{entity.FirstName} {entity.LastName}"
        };
    }

    // ändra namn alternativt byt ut
    public static UserModel CreateBasic(UserEntity entity)
    {
        return new UserModel
        {
            Id = entity.Id,
            UserImageUrl = entity.UserImageUrl,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email = entity.Email,
            Phone = entity.PhoneNumber,
            JobTitle = entity.JobTitle,
            DateOfBirth = entity.DateOfBirth,
            Address = entity.Address != null
            ? new AddressModel
            {
                AddressId = entity.Address.AddressId,
                StreetName = entity.Address.StreetName,
                StreetNumber = entity.Address.StreetNumber,
                PostalCode = entity.Address.PostalCode,
                City = entity.Address.City
            }
            : null
        };
    }

    public static UserModel CreateWithAddress(UserEntity entity)
    {
        return new UserModel
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email = entity.Email,
            Phone = entity.PhoneNumber,
            JobTitle = entity.JobTitle,
            DateOfBirth = entity.DateOfBirth,

            Address = entity.Address != null
            ? new AddressModel
            {
                AddressId = entity.Address.AddressId,
                StreetName = entity.Address.StreetName,
                StreetNumber = entity.Address.StreetNumber,
                PostalCode = entity.Address.PostalCode,
                City = entity.Address.City,
            }
            : null
        };
    }

    public static UserEntity Create(SignUpModel model)
    {
        return new UserEntity
        {
            UserName = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
        };
    }

    public static UserEntity Create(UserCreateModel user)
    {
        return new UserEntity
        {
            UserImageUrl = user.ProfileImage,
            UserName = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.Phone,
            JobTitle = user.JobTitle,
            DateOfBirth = user.DateOfBirth,
            //password?
        };
    }

    public static void Update(UserEditModel model, UserEntity entity)
    {
        entity.UserImageUrl = model.ProfileImage;
        entity.UserName = model.Email;
        entity.FirstName = model.FirstName;
        entity.LastName = model.LastName;
        entity.Email = model.Email;
        entity.PhoneNumber = model.Phone;
        entity.JobTitle = model.JobTitle;
        entity.DateOfBirth = model.DateOfBirth;
    }
}
