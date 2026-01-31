using CleanArchitecture.Entities;

namespace CleanArchitecture.EntityFrameworkCore.Abstractions.Repositories;

/// <summary>
/// Interface cho read operations của repository
/// </summary>
public interface IReadRepository<T>
    where T : BaseEntity
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    IQueryable<T> GetQueryable();
}
