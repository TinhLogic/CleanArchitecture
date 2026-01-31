using CleanArchitecture.Entities;

namespace CleanArchitecture.EntityFrameworkCore.Abstractions.Repositories;

/// <summary>
/// Interface cho bulk operations của repository
/// </summary>
public interface IBulkRepository<T>
    where T : BaseEntity
{
    Task<int> BulkDeleteAsync(
        System.Linq.Expressions.Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default
    );

    Task<int> BulkUpdateAsync<TProperty>(
        System.Linq.Expressions.Expression<Func<T, bool>> predicate,
        System.Linq.Expressions.Expression<Func<T, TProperty>> propertyExpression,
        TProperty newValue,
        CancellationToken cancellationToken = default
    );
}
