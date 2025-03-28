using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class UserRepository(DataContext context) : BaseRepository<UserEntity>(context), IUserRepository
{
    public async Task<IEnumerable<UserEntity>> GetUsersByIdAsync(List<string> ids)
    {
        return await _dbSet.Where(x => ids.Contains(x.Id)).ToListAsync();
    }
}
