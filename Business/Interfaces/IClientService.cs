using Business.Models.Clients;
using Data.Entities;

namespace Business.Interfaces;

public interface IClientService
{
    Task<(bool succeeded, int statuscode, int? clientId)> CreateClientAsync(ClientCreateModel model);
    Task<IEnumerable<ClientModel>> GetAllClientsAsync();
    Task<ClientModel?> GetClientAsync(int? id);
    Task<ClientModel> GetClientByNameAsync(string name);
    Task<ClientEntity?> GetClientEntityAsync(int? id);
    Task<int> UpdateClientAsync(ClientEditModel model);
    Task<int> DeleteClientAsync(int id);
}
