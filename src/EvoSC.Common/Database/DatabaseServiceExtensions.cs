using System.Reflection;
using EvoSC.Common.Config.Models;
using EvoSC.Common.Interfaces;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;

namespace EvoSC.Common.Database;

public static class DatabaseServiceExtensions
{
    private const int CommandTimeout = 3;
    
    public static IServiceCollection AddEvoScDatabase(this IServiceCollection services, DatabaseConfig config)
    {
        var connection = new MySqlConnection(config.GetConnectionString());
        connection.Open();
        
        var db = EvoScDb.Init(connection, CommandTimeout);

        services.AddSingleton(connection);
        services.AddSingleton(db);
        
        return services;
    }

    public static IServiceCollection AddEvoScMigrations(this IServiceCollection services)
    {
        return services.AddScoped<IMigrationManager, MigrationManager>();
    }
}
