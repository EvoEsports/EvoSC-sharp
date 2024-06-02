using System.Diagnostics;
using System.Globalization;
using EvoSC.Common.Config.Models;
using EvoSC.Common.Database.Extensions;
using EvoSC.Common.Interfaces.Database;
using LinqToDB;
using LinqToDB.Data;
using Microsoft.Extensions.Logging;

namespace EvoSC.Common.Database;

public class DbConnectionFactory(IEvoScBaseConfig config, ILogger<DbConnectionFactory> logger)
    : IDbConnectionFactory
{
    private DataConnection? _connection;

    public DataContext GetConnection()
    {
        return CreateConnection();
    }
    
    /// <summary>
    /// Create a new connection to the database.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException">Thrown when an invalid database type was specified in the config.</exception>
    private DataContext CreateConnection()
    {
        var options = CreateDatabaseOptions();
        return new DataContext(options);
    }

    /// <summary>
    /// Create a new database config builder with common configuration.
    /// </summary>
    /// <returns></returns>
    private DataOptions CreateDatabaseOptions() =>
        new DataOptions(new ConnectionOptions(
                ProviderName: config.Database.Type switch
                {
                    IDatabaseConfig.DatabaseType.MySql => ProviderName.MySql,
                    IDatabaseConfig.DatabaseType.SQLite => ProviderName.SQLite,
                    IDatabaseConfig.DatabaseType.PostgreSql => ProviderName.PostgreSQL,
                    _ => throw new InvalidOperationException("Invalid database type requested.")
                },
                ConnectionString: config.Database.GetConnectionString()
            ))
            .UseTraceLevel(config.Logging.LogLevel.ToUpper(CultureInfo.InvariantCulture) switch
            {
                "INFORMATION" => TraceLevel.Info,
                "ERROR" => TraceLevel.Error,
                "WARNING" => TraceLevel.Warning,
                "TRACE" => TraceLevel.Verbose,
                _ => TraceLevel.Off
            })
            .UseTraceWith(LogTracing);

    /// <summary>
    /// Write database tracing to the logger.
    /// </summary>
    /// <param name="s1"></param>
    /// <param name="s2"></param>
    /// <param name="level"></param>
    private void LogTracing(string? s1, string? s2, TraceLevel level)
    {
        logger.LogTrace("Database Trace: {One} | {Two}", s1, s2);
    }
}
