using CleanArchitecture.Entities;
using Microsoft.EntityFrameworkCore;

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
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    // Set CreatedAt và CreatedBy khi entity mới được thêm
                    // Note: Id và CreatedAt đã được set trong constructor
                    // Nếu cần set CreatedBy từ current user, có thể inject IHttpContextAccessor
                    break;

                case EntityState.Modified:
                    // Auto update UpdatedAt khi entity được modify
                    entry.Entity.SetUpdatedInfo();
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
