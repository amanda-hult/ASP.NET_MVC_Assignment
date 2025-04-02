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

    // CREATE
    public async Task<int> CreateClientAsync(ClientCreateModel model)
    {

        try
        {
            // check if client already exists
            bool exists = await _clientRepository.ExistsAsync(x => x.ClientName == model.ClientName);
            if (exists)
                return 409;

            // begin transaction
            await _clientRepository.BeginTransactionAsync();

            // create client
            var clientEntity = ClientFactory.Create(model);
            var createdEntity = await _clientRepository.CreateAsync(clientEntity);
            if (createdEntity == null)
            {
                await _clientRepository.RollbackTransactionAsync();
                return 500;
            }

            await _clientRepository.SaveAsync();
            await _clientRepository.CommitTransactionAsync();
            return 201;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error creating client: {ex.Message}");
            await _clientRepository.RollbackTransactionAsync();
            return 500;
        }
    }

    // READ
    public async Task<IEnumerable<ClientModel>> GetAllClientsAsync()
    {


        //get client statuses(updateclientstatusesasync();)

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

    public async Task<ClientEntity?> GetClientEntityAsync(int id)
    {
        var clientEntity = await _clientRepository.GetAsync(x => x.ClientId == id);
        if (clientEntity == null)
            return null;

        return clientEntity;
    }

    // UPDATE
    public async Task<int> UpdateClientAsync(ClientEditModel model)
    {
        var existingEntity = await _clientRepository.GetAsync(x => x.ClientId == model.Id);
        if (existingEntity == null)
            return 404;

        var updatedEntity = ClientFactory.CreateUpdated(model, existingEntity);

        var result = await _clientRepository.UpdateAsync(x => x.ClientId == model.Id, updatedEntity);
        if (result == null)
            return 500;

        return 200;
    }

    // DELETE
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
}
