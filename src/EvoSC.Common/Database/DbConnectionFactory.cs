using System.Diagnostics;
using System.Globalization;
using EvoSC.Common.Config.Models;
using EvoSC.Common.Database.Extensions;
using EvoSC.Common.Interfaces.Database;
using LinqToDB;
using LinqToDB.Configuration;
using LinqToDB.Data;
using Microsoft.Extensions.Logging;

namespace EvoSC.Common.Database;

public class DbConnectionFactory : IDbConnectionFactory
{
    private readonly IEvoScBaseConfig _config;
    private readonly object _mutex = new();
    private DataConnection? _connection;
    private readonly ILogger<DbConnectionFactory> _logger;

    public DbConnectionFactory(IEvoScBaseConfig config, ILogger<DbConnectionFactory> logger)
    {
        _config = config;
        _logger = logger;
    }
    
    public DataConnection GetConnection()
    {
        lock (_mutex)
        {
            _connection ??= CreateConnection();
            return _connection;
        }
    }
    
    /// <summary>
    /// Create a new connection to the database.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException">Thrown when an invalid database type was specified in the config.</exception>
    private DataConnection CreateConnection()
    {
        var configBuilder = CreateConfigBuilder();

        configBuilder.UseConnectionString(_config.Database.Type switch
        {
            IDatabaseConfig.DatabaseType.MySql => ProviderName.MySql,
            IDatabaseConfig.DatabaseType.SQLite => ProviderName.SQLite,
            IDatabaseConfig.DatabaseType.PostgreSql => ProviderName.PostgreSQL,
            _ => throw new InvalidOperationException("Invalid database type requested.")
        }, _config.Database.GetConnectionString());
        return new DataConnection(configBuilder.Build());
    }

    /// <summary>
    /// Create a new database config builder with common configuration.
    /// </summary>
    /// <returns></returns>
    private LinqToDBConnectionOptionsBuilder CreateConfigBuilder() =>
        new LinqToDBConnectionOptionsBuilder()
            .WithTraceLevel(_config.Logging.LogLevel.ToUpper(CultureInfo.InvariantCulture) switch
            {
                "INFORMATION" => TraceLevel.Info,
                "ERROR" => TraceLevel.Error,
                "WARNING" => TraceLevel.Warning,
                "TRACE" => TraceLevel.Verbose,
                _ => TraceLevel.Off
            })
            .WriteTraceWith(LogTracing);

    /// <summary>
    /// Write database tracing to the logger.
    /// </summary>
    /// <param name="s1"></param>
    /// <param name="s2"></param>
    /// <param name="level"></param>
    private void LogTracing(string? s1, string? s2, TraceLevel level)
    {
        _logger.LogTrace("Database Trace: {One} | {Two}", s1, s2);
    }
}
