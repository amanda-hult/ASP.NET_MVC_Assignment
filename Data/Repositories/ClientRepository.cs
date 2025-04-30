using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class ClientRepository(DataContext context) : BaseRepository<ClientEntity>(context), IClientRepository
{
    public async Task<IEnumerable<ClientEntity>> GetClientsByStringAsync(string term)
    {
        return await _dbSet.Where(x => x.ClientName.Contains(term) || x.Email.Contains(term)).ToListAsync();
    }
}
