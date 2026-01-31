using CleanArchitecture.EntityFrameworkCore.Abstractions;
using CleanArchitecture.EntityFrameworkCore.Abstractions.Repositories;
using CleanArchitecture.EntityFrameworkCore.Configuration;
using CleanArchitecture.EntityFrameworkCore.Implementation.Repositories;
using CleanArchitecture.EntityFrameworkCore.Implementation.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.EntityFrameworkCore;

/// <summary>
/// Extension methods để register Infrastructure services
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure<TDbContext>(
        this IServiceCollection services,
        IConfiguration configuration
    )
        where TDbContext : DbContext
    {
        // Đọc danh sách databases từ config
        DatabaseConfiguration? dbConfig = configuration.GetSection("Databases").Get<DatabaseConfiguration>();
        if (dbConfig == null)
        {
            throw new InvalidOperationException("No database configuration found in appsettings");
        }

        // Register DbContext với provider phù hợp
        services.AddDbContext<TDbContext>(options =>
        {
            switch (dbConfig.Type)
            {
                case DatabaseProvider.SqlServer:
                    options.UseSqlServer(
                        dbConfig.ConnectionString,
                        b => b.MigrationsAssembly(typeof(TDbContext).Assembly.FullName)
                    );
                    break;

                case DatabaseProvider.PostgreSql:
                    options.UseNpgsql(
                        dbConfig.ConnectionString,
                        b => b.MigrationsAssembly(typeof(TDbContext).Assembly.FullName)
                    );
                    break;

                default:
                    throw new NotSupportedException(
                        $"Database provider {dbConfig.Type} is not supported"
                    );
            }
        });

        // Register DbContext as base DbContext
        services.AddScoped<DbContext>(provider => provider.GetRequiredService<TDbContext>());

        // Register UnitOfWork
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Register generic repository
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        return services;
    }

    public static IServiceCollection AddInfrastructureWithOptions<TDbContext>(
        this IServiceCollection services,
        Action<DbContextOptionsBuilder> optionsAction
    )
        where TDbContext : DbContext
    {
        // Register DbContext với custom options
        services.AddDbContext<TDbContext>(optionsAction);

        // Register DbContext as base DbContext
        services.AddScoped<DbContext>(provider => provider.GetRequiredService<TDbContext>());

        // Register UnitOfWork
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Register generic repository
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        return services;
    }
}
