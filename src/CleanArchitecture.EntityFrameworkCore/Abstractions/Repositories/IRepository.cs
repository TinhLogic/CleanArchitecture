using CleanArchitecture.Entities;

namespace CleanArchitecture.EntityFrameworkCore.Abstractions.Repositories;

/// <summary>
/// Base interface cho tất cả các repository.
/// Kế thừa từ IReadRepository, IWriteRepository và IBulkRepository
/// để tuân thủ Interface Segregation Principle.
/// </summary>
public interface IRepository<T> : IReadRepository<T>, IWriteRepository<T>, IBulkRepository<T>
    where T : BaseEntity { }
