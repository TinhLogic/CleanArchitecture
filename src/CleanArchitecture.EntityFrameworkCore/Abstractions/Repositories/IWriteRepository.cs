using CleanArchitecture.Entities;

namespace CleanArchitecture.EntityFrameworkCore.Abstractions.Repositories;

/// <summary>
/// Interface cho write operations của repository
/// </summary>
public interface IWriteRepository<T>
    where T : BaseEntity
{
    public Task<T> InsertAsync(T entity, CancellationToken cancellationToken = default);
    public Task InsertManyAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    public Task EditAsync(T entity, CancellationToken cancellationToken = default);
    public Task EditManyAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    public Task RemoveAsync(T entity, CancellationToken cancellationToken = default);
    public Task RemoveManyAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
}
