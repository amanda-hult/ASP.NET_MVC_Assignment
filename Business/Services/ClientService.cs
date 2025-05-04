using System.Diagnostics;
using Business.Factories;
using Business.Interfaces;
using Business.Models.Clients;
using Data.Entities;
using Data.Interfaces;

namespace Business.Services;

public class ClientService(IClientRepository clientRepository, IProjectRepository projectRepository) : IClientService
{
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly IProjectRepository _projectRepository = projectRepository;

    #region Create
    public async Task<(bool succeeded, int statuscode, int? clientId)> CreateClientAsync(ClientCreateModel model)
    {
        // check if client already exists
        bool exists = await _clientRepository.ExistsAsync(x => x.ClientName == model.ClientName);
        if (exists)
            return (false, 409, null);

        await _clientRepository.BeginTransactionAsync();
        try
        {
            var createdEntity = await _clientRepository.CreateAsync(ClientFactory.Create(model));
            if (createdEntity == null)
            {
                await _clientRepository.RollbackTransactionAsync();
                return (false, 500, null);
            }

            await _clientRepository.SaveAsync();
            await _clientRepository.CommitTransactionAsync();
            return (true, 201, createdEntity.ClientId);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error creating client: {ex}");
            await _clientRepository.RollbackTransactionAsync();
            return (false, 500, null);
        }
    }
    #endregion

    #region Read
    public async Task<IEnumerable<ClientModel>> GetAllClientsAsync()
    {
        var list = await _clientRepository.GetAllAsync();
        if (list == null)
            return null;

        var clients = list.Select(ClientFactory.Create).ToList();
        return clients;
    }

    public async Task<ClientModel> GetClientAsync(int? id)
    {
        var clientEntity = await _clientRepository.GetAsync(x => x.ClientId == id);
        if (clientEntity == null)
            return null;

        var clientModel = ClientFactory.Create(clientEntity);
        return clientModel;
    }

    public async Task<ClientEntity?> GetClientEntityAsync(int? id)
    {
        var clientEntity = await _clientRepository.GetAsync(x => x.ClientId == id);
        if (clientEntity == null)
            return null;

        return clientEntity;
    }
    #endregion

    #region Update
    public async Task<int> UpdateClientAsync(ClientEditModel model)
    {
        bool exists = await _clientRepository.ExistsAsync(x => x.ClientName == model.ClientName && x.ClientId != model.Id);
        if (exists)
            return 409;

        try
        {
            var existingEntity = await _clientRepository.GetAsync(x => x.ClientId == model.Id);
            if (existingEntity == null)
                return 404;

            var updatedEntity = ClientFactory.CreateUpdated(model);

            await _clientRepository.BeginTransactionAsync();

            var result = await _clientRepository.UpdateAsync(x => x.ClientId == model.Id, updatedEntity);
            if (result == null)
            {
                await _clientRepository.RollbackTransactionAsync();
                return 500;
            }

            await _clientRepository.SaveAsync();
            await _clientRepository.CommitTransactionAsync();

            return 200;
        }
        catch (Exception ex)
        {
            await _clientRepository.RollbackTransactionAsync();
            Debug.WriteLine($"Error updating client: {ex.Message}");
            return 500;
        }
    }
    #endregion

    #region Delete
    public async Task<int> DeleteClientAsync(int id)
    {
        var clientExists = await _clientRepository.ExistsAsync(x => x.ClientId == id);
        if (!clientExists)
            return 404;

        bool existsInProject = await _projectRepository.ExistsAsync(x => x.ClientId == id);
        if (existsInProject)
            return 409;

        bool result = await _clientRepository.DeleteAsync(x => x.ClientId == id);
        if (!result)
            return 500;

        return 204;
    }
    #endregion
}
