using CleanArchitecture.Entities;

namespace CleanArchitecture.EntityFrameworkCore.Abstractions.Repositories;

/// <summary>
/// Interface cho read operations của repository
/// </summary>
public interface IReadRepository<T>
    where T : BaseEntity
{
    public Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    public Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    public IQueryable<T> GetQueryable();
}
