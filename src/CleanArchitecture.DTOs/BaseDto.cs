namespace CleanArchitecture.DTOs;

/// <summary>
/// Base DTO class
/// </summary>
public abstract class BaseDto
{
    public Guid Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
}
