namespace CleanArchitecture.EntityFrameworkCore.Abstractions;

/// <summary>
/// Unit of Work interface để quản lý transactions
/// </summary>
public interface IUnitOfWork : IDisposable
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    public Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    public Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    public Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
