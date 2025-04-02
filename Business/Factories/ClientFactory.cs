using Business.Models.Clients;
using Data.Entities;

namespace Business.Factories;

public static class ClientFactory
{
    public static ClientEntity Create(ClientCreateModel model)
    {
        return new ClientEntity
        {
            ClientName = model.ClientName,
            Email = model.Email,
            Location = model.Location,
            Phone = model.Phone,
            //Status = "inactive"
        };
    }

    public static ClientModel Create(ClientEntity entity)
    {
        return new ClientModel
        {
            Id = entity.ClientId,
            ClientName = entity.ClientName,
            Email = entity.Email,
            Location = entity.Location,
            Phone = entity.Phone,
            Created = entity.Created,
            //Status = entity.Status,
        };
    }

    public static ClientEntity CreateUpdated(ClientEditModel model, ClientEntity existingEntity)
    {
        return new ClientEntity
        {
            ClientId = existingEntity.ClientId,
            Created = existingEntity.Created,
            //Status = existingEntity.Status,
            Projects = existingEntity.Projects,

            ClientName = model.ClientName,
            Email = model.Email,
            Location = model.Location,
            Phone = model.Phone,
        };
    }
}
