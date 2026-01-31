using CleanArchitecture.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CleanArchitecture.EntityFrameworkCore.Implementation.DbContexts;

/// <summary>
/// Base DbContext cho application
/// </summary>
public abstract class BaseDbContext : DbContext
{
    protected BaseDbContext(DbContextOptions options)
        : base(options) { }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Auto set audit fields cho các entity
        foreach (EntityEntry<BaseEntity> entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    // Hook method cho derived class override để set CreatedBy từ current user
                    SetCreatedAuditFields(entry.Entity);
                    break;

                case EntityState.Modified:
                    // Hook method cho derived class override để set UpdatedBy từ current user
                    SetModifiedAuditFields(entry.Entity);
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    protected virtual void SetCreatedAuditFields(BaseEntity entity)
    {
    }

    protected virtual void SetModifiedAuditFields(BaseEntity entity)
    {
        entity.SetUpdatedInfo();
    }
}
