using Business.Models;
using Data.Entities;

namespace Business.Interfaces;

public interface IClientService
{
    Task<int> CreateClientAsync(ClientCreateModel model);
    Task<IEnumerable<ClientModel>> GetAllClientsAsync();
    Task<ClientModel?> GetClientAsync(int? id);
    Task<ClientEntity?> GetClientEntityAsync(int id);
    Task<int> UpdateClientAsync(ClientEditModel model);
    Task<int> DeleteClientAsync(int id);
}
