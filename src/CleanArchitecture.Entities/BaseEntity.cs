namespace CleanArchitecture.Entities;

/// <summary>
/// Base entity class cho tất cả các entity trong domain
/// </summary>
public abstract class BaseEntity
{
    public Guid Id { get; protected set; }
    public DateTimeOffset CreatedAt { get; protected set; }
    public DateTimeOffset? UpdatedAt { get; protected set; }
    public string? CreatedBy { get; protected set; }
    public string? UpdatedBy { get; protected set; }

    protected BaseEntity()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Set thông tin UpdatedAt và UpdatedBy khi entity được modify
    /// </summary>
    public void SetUpdatedInfo(string? updatedBy = null)
    {
        UpdatedAt = DateTimeOffset.UtcNow;
        if (updatedBy != null)
        {
            UpdatedBy = updatedBy;
        }
    }
}
