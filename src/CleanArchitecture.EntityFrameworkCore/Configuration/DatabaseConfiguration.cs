namespace CleanArchitecture.EntityFrameworkCore.Configuration;

/// <summary>
/// Configuration cho database connection
/// </summary>
public class DatabaseConfiguration
{
    public DatabaseProvider Type { get; set; } = DatabaseProvider.SqlServer;
    public string ConnectionString { get; set; } = string.Empty;
}

/// <summary>
/// Database provider types
/// </summary>
public enum DatabaseProvider
{
    SqlServer,
    PostgreSql,
}
