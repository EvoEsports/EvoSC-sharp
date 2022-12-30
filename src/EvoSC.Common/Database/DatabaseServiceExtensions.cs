using System.Data.Common;
using System.Reflection;
using Dapper;
using Dapper.FluentMap;
using EvoSC.Common.Config.Models;
using EvoSC.Common.Database.Util;
using EvoSC.Common.Interfaces;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Generators;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;
using Npgsql;
using SimpleInjector;

namespace EvoSC.Common.Database;

public static class DatabaseServiceExtensions
{
    private const int CommandTimeout = 3;
    
    public static Container AddEvoScDatabase(this Container services, IDatabaseConfig config)
    {
        DbConnection connection;
        switch (config.Type)
        {
            case IDatabaseConfig.DatabaseType.MySql:
                connection = new MySqlConnection(config.GetConnectionString());
                break;
            case IDatabaseConfig.DatabaseType.SQLite:
                connection = new SqliteConnection(config.GetConnectionString());
                break;
            default:
                FluentMapper.Initialize(conf =>
                {
                    conf.AddConvention<PropertyTransformConvention>();
                });
                connection = new NpgsqlConnection(config.GetConnectionString());
                
                break;
        }
        
        connection.Open();

        services.RegisterInstance(connection);

        return services;
    }

    public static Container AddEvoScMigrations(this Container services)
    {
        services.Register<IMigrationManager, MigrationManager>(Lifestyle.Scoped);
        return services;
    }
}
