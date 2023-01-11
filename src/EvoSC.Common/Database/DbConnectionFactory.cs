using System.Data;
using System.Data.Common;
using EvoSC.Common.Config.Models;
using EvoSC.Common.Database.Extensions;
using EvoSC.Common.Interfaces.Database;
using Microsoft.Data.Sqlite;
using MySql.Data.MySqlClient;
using Npgsql;
using RepoDb;
using SqlKata.Compilers;

namespace EvoSC.Common.Database;

public class DbConnectionFactory : IDbConnectionFactory
{
    private readonly IEvoScBaseConfig _config;
    private readonly object _mutex = new();
    private DbConnection? _connection;
    
    public DbConnectionFactory(IEvoScBaseConfig config)
    {
        _config = config;
    }

    public DbConnection GetConnection()
    {
        lock (_mutex)
        {
            _connection ??= OpenConnection();

            if (_connection.State is ConnectionState.Broken or ConnectionState.Closed)
            {
                _connection.Open();
            }

            return _connection;
        }
    }

    public Compiler GetSqlCompiler() => _config.Database.Type switch
    {
        IDatabaseConfig.DatabaseType.MySql => new MySqlCompiler(),
        IDatabaseConfig.DatabaseType.SQLite => new SqliteCompiler(),
        IDatabaseConfig.DatabaseType.PostgreSql => new PostgresCompiler(),
        _ => throw new InvalidOperationException("Invalid database type requested.")
    };

    private DbConnection OpenConnection()
    {
        DbConnection connection;
        switch (_config.Database.Type)
        {
            case IDatabaseConfig.DatabaseType.MySql:
                connection = new MySqlConnection(_config.Database.GetConnectionString());
                GlobalConfiguration.Setup().UseMySql().UseMySqlConnector();
                break;
            case IDatabaseConfig.DatabaseType.SQLite:
                connection = new SqliteConnection(_config.Database.GetConnectionString());
                GlobalConfiguration.Setup().UseSqlite();
                break;
            case IDatabaseConfig.DatabaseType.PostgreSql:
                connection = new NpgsqlConnection(_config.Database.GetConnectionString());
                GlobalConfiguration.Setup().UsePostgreSql();
                break;
            default:
                throw new InvalidOperationException("Invalid database type requested.");
        }
        
        connection.Open();
        return connection;
    }
}
