using System.Diagnostics;
using System.Linq.Expressions;
using Data.Contexts;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Data.Repositories;

public abstract class BaseRepository<TEntity>(DataContext context) : IBaseRepository<TEntity> where TEntity : class
{
    protected readonly DataContext _context = context;
    protected readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

    // TRANSACTION MANAGEMENT
    private IDbContextTransaction _transaction = null!;

    public virtual async Task BeginTransactionAsync()
    {
        if (_transaction == null)
        {
            _transaction = await _context.Database.BeginTransactionAsync();
            _context.Database.UseTransaction(_transaction.GetDbTransaction());
        }
    }

    public virtual async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null!;
        }
    }

    public virtual async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null!;
        }
    }

    // CREATE
    public virtual async Task<TEntity> CreateAsync(TEntity entity)
    {
        if (entity == null)
            return null!;

        try
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error creating entity: {ex.Message}");
            return null!;
        }
    }
    // READ
    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> includeExpression = null)
    {
        IQueryable<TEntity> query = _dbSet;

        if (includeExpression != null)
        {
            query = includeExpression(query);
        }

        return await query.ToListAsync();
    }

    public virtual async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeExpression = null)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        IQueryable<TEntity> query = _dbSet;

        if (includeExpression != null)
        {
            query = includeExpression(query);
        }

        return await query.FirstOrDefaultAsync(predicate);
    }

    // UPDATE
    public virtual async Task<TEntity> UpdateAsync(Expression<Func<TEntity, bool>> predicate, TEntity updatedEntity)
    {
        if (updatedEntity == null)
            return null;

        try
        {
            var entityToUpdate = await _dbSet.FirstOrDefaultAsync(predicate);
            if (entityToUpdate == null)
                return null;

            _context.Entry(entityToUpdate).CurrentValues.SetValues(updatedEntity);
            await _context.SaveChangesAsync();
            return entityToUpdate;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error updating entity: {ex.Message}");
            return null!;
        }
    }

    // DELETE
    public virtual async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            var entityToDelete = await _dbSet.FirstOrDefaultAsync(predicate);
            if (entityToDelete == null)
                return false;

            _dbSet.Remove(entityToDelete);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error deleting entity: {ex.Message}");
            throw;
        }
    }

    #region Exists
    public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            return await _dbSet.AnyAsync(predicate);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error checking existence: {ex.Message}");
            throw;
        }
    }
    #endregion

    // SAVE
    public virtual async Task<int> SaveAsync()
    {
        try
        {
            return await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error saving changes: {ex.Message}");
            throw;
        }

    }
}
