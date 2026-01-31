using CleanArchitecture.DTOs;
using CleanArchitecture.Entities;
using CleanArchitecture.EntityFrameworkCore.Models;

namespace CleanArchitecture.BO.Interfaces;

/// <summary>
/// Base service interface với CRUD operations
/// </summary>
public interface IBaseService<TEntity, TDto> : IService
    where TEntity : BaseEntity
    where TDto : BaseDto
{
    Task<Result<TDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Result<IEnumerable<TDto>>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Result<TDto>> CreateAsync(TDto dto, CancellationToken cancellationToken = default);

    Task<Result<TDto>> UpdateAsync(
        Guid id,
        TDto dto,
        CancellationToken cancellationToken = default
    );

    Task<Result<TDto>> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    IQueryable<TEntity> GetQueryable();
}
