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
    public Task<Result<TDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    public Task<Result<IEnumerable<TDto>>> GetAllAsync(CancellationToken cancellationToken = default);

    public Task<Result<TDto>> CreateAsync(TDto dto, CancellationToken cancellationToken = default);

    public Task<Result<TDto>> UpdateAsync(
        Guid id,
        TDto dto,
        CancellationToken cancellationToken = default
    );

    public Task<Result<TDto>> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    public IQueryable<TEntity> GetQueryable();
}
