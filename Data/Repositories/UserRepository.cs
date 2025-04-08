using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class UserRepository(DataContext context) : BaseRepository<UserEntity>(context), IUserRepository
{

    // byt eventuellt ut denna metod mot nedan
    public async Task<IEnumerable<UserEntity>> GetUsersByIdAsync(List<string> ids)
    {
        return await _dbSet.Where(x => ids.Contains(x.Id)).ToListAsync();
    }

    public async Task<IEnumerable<UserEntity>> GetUsersByStringAsync(string term)
    {
        return await _dbSet
            .Where(x => x.FirstName.Contains(term) || x.LastName.Contains(term) || x.Email.Contains(term))
            .ToListAsync();
    }
}
