using System.Diagnostics;
using System.Linq.Expressions;
using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class ProjectRepository(DataContext context) : BaseRepository<ProjectEntity>(context), IProjectRepository
{
    public async Task<bool> UserExistsInProject(string userId)
    {
        return await _dbSet.AnyAsync(x => x.ProjectUsers.Any(x => x.UserId == userId));
    }

    public virtual async Task<ProjectEntity> UpdateProjectAsync(Expression<Func<ProjectEntity, bool>> predicate, ProjectEntity updatedEntity)
    {
        if (updatedEntity == null)
            return null;

        try
        {
            var entityToUpdate = await _dbSet
                .Include(p => p.Client)
                .Include(p => p.Status)
                .Include(p => p.ProjectUsers)
                    .ThenInclude(pu => pu.User)
                .FirstOrDefaultAsync(predicate);

            if (entityToUpdate == null)
                return null;

            _context.Entry(entityToUpdate).CurrentValues.SetValues(updatedEntity);
            await _context.SaveChangesAsync();
            return entityToUpdate;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error updating projectEntity: {ex.Message}");
            return null!;
        }
    }
}
