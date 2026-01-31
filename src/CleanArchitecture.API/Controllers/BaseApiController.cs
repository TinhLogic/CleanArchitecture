using CleanArchitecture.BO.Interfaces;
using CleanArchitecture.DTOs;
using CleanArchitecture.Entities;
using CleanArchitecture.EntityFrameworkCore.Models;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.API.Controllers;

/// <summary>
/// Base API Controller with CRUD operations
/// </summary>
/// <typeparam name="TEntity">Entity type that inherits from BaseEntity</typeparam>
/// <typeparam name="TDto">DTO type that inherits from BaseDto</typeparam>
[ApiController]
[Route("api/[controller]")]
public class BaseApiController<TEntity, TDto> : ControllerBase
    where TEntity : BaseEntity
    where TDto : BaseDto
{
    private readonly IBaseService<TEntity, TDto> _service;

    public BaseApiController(IBaseService<TEntity, TDto> service)
    {
        _service = service;
    }

    /// <summary>
    /// Map result code to appropriate HTTP status code
    /// </summary>
    protected ActionResult<T> ToActionResult<T>(T result)
        where T : notnull
    {
        if (result is Result<TDto> typedResult)
        {
            return typedResult.Code switch
            {
                200 => Ok(result),
                201 => Created(string.Empty, result),
                400 => BadRequest(result),
                401 => Unauthorized(result),
                403 => StatusCode(403, result),
                404 => NotFound(result),
                500 => StatusCode(500, result),
                _ => StatusCode(typedResult.Code, result),
            };
        }

        if (result is Result plainResult)
        {
            return plainResult.Code switch
            {
                200 => Ok(result),
                400 => BadRequest(result),
                401 => Unauthorized(result),
                403 => StatusCode(403, result),
                404 => NotFound(result),
                500 => StatusCode(500, result),
                _ => StatusCode(plainResult.Code, result),
            };
        }

        // For IEnumerable results
        var codeProperty = result.GetType().GetProperty("Code");
        if (codeProperty != null && codeProperty.GetValue(result) is int code)
        {
            return code switch
            {
                200 => Ok(result),
                400 => BadRequest(result),
                401 => Unauthorized(result),
                403 => StatusCode(403, result),
                404 => NotFound(result),
                500 => StatusCode(500, result),
                _ => StatusCode(code, result),
            };
        }

        return Ok(result);
    }

    /// <summary>
    /// Get all entities
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<Result<IEnumerable<TDto>>>> GetAll()
    {
        try
        {
            var result = await _service.GetAllAsync();
            return ToActionResult(result);
        }
        catch (Exception ex)
        {
            var errorResult = Result<IEnumerable<TDto>>.Failure(ex.Message, code: 500);
            return StatusCode(500, errorResult);
        }
    }

    /// <summary>
    /// Get entity by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<Result<TDto>>> GetById(Guid id)
    {
        try
        {
            var result = await _service.GetByIdAsync(id);
            return ToActionResult(result);
        }
        catch (Exception ex)
        {
            var errorResult = Result<TDto>.Failure(ex.Message, code: 500);
            return StatusCode(500, errorResult);
        }
    }

    /// <summary>
    /// Create new entity
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Result<TDto>>> Create([FromBody] TDto dto)
    {
        try
        {
            var result = await _service.CreateAsync(dto);
            if (result.Code == 201)
                return CreatedAtAction(nameof(GetById), new { id = result.Data?.Id }, result);
            return ToActionResult(result);
        }
        catch (Exception ex)
        {
            var errorResult = Result<TDto>.Failure(ex.Message, code: 500);
            return StatusCode(500, errorResult);
        }
    }

    /// <summary>
    /// Update entity
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<Result<TDto>>> Update(Guid id, [FromBody] TDto dto)
    {
        try
        {
            if (id != dto.Id)
                return BadRequest(Result<TDto>.Failure("ID mismatch"));

            var result = await _service.UpdateAsync(id, dto);
            return ToActionResult(result);
        }
        catch (Exception ex)
        {
            var errorResult = Result<TDto>.Failure(ex.Message, code: 500);
            return StatusCode(500, errorResult);
        }
    }

    /// <summary>
    /// Delete entity
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<Result<TDto>>> Delete(Guid id)
    {
        try
        {
            var result = await _service.DeleteAsync(id);
            return ToActionResult(result);
        }
        catch (Exception ex)
        {
            var errorResult = Result<TDto>.Failure(ex.Message, code: 500);
            return StatusCode(500, errorResult);
        }
    }

    /// <summary>
    /// Filter entities with DevExtreme DataSource options
    /// </summary>
    [HttpPost("filter")]
    public virtual async Task<ActionResult<LoadResult>> Filter(
        [FromBody] DataSourceLoadOptionsBase loadOptions
    )
    {
        try
        {
            var queryable = _service.GetQueryable();
            var result = await DataSourceLoader.LoadAsync(queryable, loadOptions);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    /// <summary>
    /// Override this method to provide queryable data for filtering
    /// </summary>
    protected virtual IQueryable<TDto> GetQueryableForFilter()
    {
        throw new NotImplementedException(
            "Override GetQueryableForFilter in derived controller to enable filtering"
        );
    }
}
