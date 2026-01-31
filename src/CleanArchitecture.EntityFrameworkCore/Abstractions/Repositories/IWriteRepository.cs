using CleanArchitecture.Entities;

namespace CleanArchitecture.EntityFrameworkCore.Abstractions.Repositories;

/// <summary>
/// Interface cho write operations của repository
/// </summary>
public interface IWriteRepository<T>
    where T : BaseEntity
{
    Task<T> InsertAsync(T entity, CancellationToken cancellationToken = default);
    Task InsertManyAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    Task EditAsync(T entity, CancellationToken cancellationToken = default);
    Task EditManyAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    Task RemoveAsync(T entity, CancellationToken cancellationToken = default);
    Task RemoveManyAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
}
