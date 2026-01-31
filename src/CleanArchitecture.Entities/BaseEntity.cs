namespace CleanArchitecture.Entities;

/// <summary>
/// Base entity class cho tất cả các entity trong domain
/// </summary>
public abstract class BaseEntity
{
    public Guid Id { get; protected set; }
    public DateTimeOffset CreatedAt { get; protected set; }
    public DateTimeOffset? UpdatedAt { get; protected set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    protected BaseEntity()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public void SetUpdatedInfo()
    {
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
