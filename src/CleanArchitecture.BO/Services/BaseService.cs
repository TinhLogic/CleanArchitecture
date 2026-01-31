using CleanArchitecture.BO.Interfaces;
using CleanArchitecture.DTOs;
using CleanArchitecture.Entities;
using CleanArchitecture.EntityFrameworkCore.Abstractions;
using CleanArchitecture.EntityFrameworkCore.Abstractions.Repositories;
using CleanArchitecture.EntityFrameworkCore.Models;

namespace CleanArchitecture.BO.Services;

/// <summary>
/// Base service implementation với CRUD operations
/// </summary>
public abstract class BaseService<TEntity, TDto> : IBaseService<TEntity, TDto>
    where TEntity : BaseEntity
    where TDto : BaseDto
{
    protected readonly IRepository<TEntity> _repository;
    protected readonly IUnitOfWork _unitOfWork;

    protected BaseService(IRepository<TEntity> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public virtual async Task<Result<TDto>> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        if (entity == null)
        {
            return Result<TDto>.Failure(message: $"Entity with id {id} not found", code: 404);
        }

        var dto = MapToDto(entity);
        return Result<TDto>.Success(dto);
    }

    public virtual async Task<Result<IEnumerable<TDto>>> GetAllAsync(
        CancellationToken cancellationToken = default
    )
    {
        var entities = await _repository.GetAllAsync(cancellationToken);
        var dtos = entities.Select(MapToDto);
        return Result<IEnumerable<TDto>>.Success(dtos);
    }

    public virtual async Task<Result<TDto>> CreateAsync(
        TDto dto,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(dto);

        var entity = MapToEntity(dto);
        await _repository.InsertAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var resultDto = MapToDto(entity);
        return Result<TDto>.Success(resultDto, code: 201);
    }

    public virtual async Task<Result<TDto>> UpdateAsync(
        Guid id,
        TDto dto,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(dto);

        var existingEntity = await _repository.GetByIdAsync(id, cancellationToken);
        if (existingEntity == null)
        {
            return Result<TDto>.Failure($"Entity with id {id} not found", code: 404);
        }

        UpdateEntity(existingEntity, dto);
        await _repository.EditAsync(existingEntity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var resultDto = MapToDto(existingEntity);
        return Result<TDto>.Success(resultDto);
    }

    public virtual async Task<Result<TDto>> DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        if (entity == null)
        {
            return Result<TDto>.Failure($"Entity with id {id} not found", code: 404);
        }

        var dto = MapToDto(entity);
        await _repository.RemoveAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<TDto>.Success(dto, "Entity deleted successfully");
    }

    // Abstract methods để subclass implement
    protected abstract TDto MapToDto(TEntity entity);
    protected abstract TEntity MapToEntity(TDto dto);
    protected abstract void UpdateEntity(TEntity entity, TDto dto);

    public IQueryable<TEntity> GetQueryable()
    {
        return _repository.GetQueryable();
    }
}
