using System.Data.Common;
using System.Reflection;
using Dapper;
using EvoSC.Common.Config.Models;
using EvoSC.Common.Interfaces;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;
using SimpleInjector;

namespace EvoSC.Common.Database;

public static class DatabaseServiceExtensions
{
    private const int CommandTimeout = 3;
    
    public static Container AddEvoScDatabase(this Container services, IDatabaseConfig config)
    {
        var connection = new MySqlConnection(config.GetConnectionString());
        connection.Open();

        services.RegisterInstance<DbConnection>(connection);
        
        return services;
    }

    public static Container AddEvoScMigrations(this Container services)
    {
        services.Register<IMigrationManager, MigrationManager>(Lifestyle.Scoped);
        return services;
    }
}
