using CleanArchitecture.Entities;
using CleanArchitecture.EntityFrameworkCore.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.EntityFrameworkCore.Implementation.Repositories;

/// <summary>
/// Generic repository implementation
/// </summary>
public class Repository<T> : IRepository<T>
    where T : BaseEntity
{
    protected readonly DbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(DbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        return await _dbSet.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync(
        CancellationToken cancellationToken = default
    )
    {
        return await _dbSet.ToListAsync(cancellationToken);
    }

    public virtual async Task<T> InsertAsync(T entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        await _dbSet.AddAsync(entity, cancellationToken);
        return entity;
    }

    public virtual async Task InsertManyAsync(
        IEnumerable<T> entities,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(entities);
        await _dbSet.AddRangeAsync(entities, cancellationToken);
    }

    public virtual Task EditAsync(T entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        _dbSet.Update(entity);
        return Task.CompletedTask;
    }

    public virtual Task EditManyAsync(
        IEnumerable<T> entities,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(entities);
        _dbSet.UpdateRange(entities);
        return Task.CompletedTask;
    }

    public virtual Task RemoveAsync(T entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        _dbSet.Remove(entity);
        return Task.CompletedTask;
    }

    public virtual Task RemoveManyAsync(
        IEnumerable<T> entities,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(entities);
        _dbSet.RemoveRange(entities);
        return Task.CompletedTask;
    }

    public virtual async Task<bool> ExistsAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        return await _dbSet.AnyAsync(e => e.Id == id, cancellationToken);
    }

    public virtual IQueryable<T> GetQueryable()
    {
        return _dbSet.AsQueryable();
    }

    // Bulk operations - EF Core Advanced
    /// <summary>
    /// Bulk delete entities matching predicate without loading them into memory
    /// </summary>
    public virtual async Task<int> BulkDeleteAsync(
        System.Linq.Expressions.Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(predicate);
        return await _dbSet.Where(predicate).ExecuteDeleteAsync(cancellationToken);
    }

    /// <summary>
    /// Bulk update a single property for entities matching predicate without loading them into memory
    /// </summary>
    public virtual async Task<int> BulkUpdateAsync<TProperty>(
        System.Linq.Expressions.Expression<Func<T, bool>> predicate,
        System.Linq.Expressions.Expression<Func<T, TProperty>> propertyExpression,
        TProperty newValue,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(propertyExpression);

        return await _dbSet
            .Where(predicate)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(propertyExpression, newValue),
                cancellationToken
            );
    }
}
