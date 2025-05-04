using Business.Models.Clients;
using Data.Entities;

namespace Business.Factories;

public static class ClientFactory
{
    public static ClientEntity Create(ClientCreateModel model)
    {
        return new ClientEntity
        {
            ClientImageUrl = model.ClientImage,
            ClientName = model.ClientName,
            Email = model.Email,
            Location = model.Location,
            Phone = model.Phone
        };
    }

    public static ClientModel Create(ClientEntity entity)
    {
        return new ClientModel
        {
            Id = entity.ClientId,
            ClientImage = entity.ClientImageUrl,
            ClientName = entity.ClientName,
            Email = entity.Email,
            Location = entity.Location,
            Phone = entity.Phone,
            Created = entity.Created
        };
    }

    public static ClientEntity CreateUpdated(ClientEditModel model)
    {
        return new ClientEntity
        {
            ClientId = model.Id,
            ClientImageUrl = model.ClientImage,
            ClientName = model.ClientName,
            Email = model.Email,
            Location = model.Location,
            Phone = model.Phone,
        };
    }
}
