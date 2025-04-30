using Data.Entities;

namespace Data.Interfaces;

public interface IClientRepository : IBaseRepository<ClientEntity>
{
    Task<IEnumerable<ClientEntity>> GetClientsByStringAsync(string term);
}
