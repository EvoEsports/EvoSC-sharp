﻿using System.Data;
using System.Data.Common;
using EvoSC.Common.Config.Models;
using EvoSC.Common.Database.Repository.Maps;
using EvoSC.Common.Database.Repository.Permissions;
using EvoSC.Common.Database.Repository.Players;
using EvoSC.Common.Database.Repository.Stores;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Database;
using EvoSC.Common.Interfaces.Database.Repository;
using Microsoft.Data.Sqlite;
using MySqlConnector;
using Npgsql;
using RepoDb;
using SimpleInjector;

namespace EvoSC.Common.Database;

public static class DatabaseServiceExtensions
{
    private const int CommandTimeout = 3;
    
    public static Container AddEvoScDatabase(this Container services, IDatabaseConfig config)
    {
        services.RegisterSingleton<IDbConnectionFactory, DbConnectionFactory>();
        services.Register<IConfigStoreRepository, ConfigStoreRepository>(Lifestyle.Transient);
        services.Register<IMapRepository, MapRepository>(Lifestyle.Transient);
        services.Register<IPermissionRepository, PermissionRepository>(Lifestyle.Transient);
        services.Register<IPlayerRepository, PlayerRepository>();

        return services;
    }

    public static Container AddEvoScMigrations(this Container services)
    {
        services.Register<IMigrationManager, MigrationManager>(Lifestyle.Scoped);
        return services;
    }
}
