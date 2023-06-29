using EvoSC.Common.Application;
using EvoSC.Common.Config.Models;
using EvoSC.Common.Database.Repository.Audit;
using EvoSC.Common.Database.Repository.Maps;
using EvoSC.Common.Database.Repository.Permissions;
using EvoSC.Common.Database.Repository.Players;
using EvoSC.Common.Database.Repository.Stores;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Database;
using EvoSC.Common.Interfaces.Database.Repository;
using SimpleInjector;

namespace EvoSC.Common.Database.Extensions;

public static class DatabaseServiceExtensions
{
    public static ServicesBuilder AddEvoScDatabase(this ServicesBuilder services, IDatabaseConfig config)
    {
        services.Register<IDbConnectionFactory, DbConnectionFactory>();
        
        services.Register<IConfigStoreRepository, ConfigStoreRepository>(Lifestyle.Transient);
        services.Register<IMapRepository, MapRepository>(Lifestyle.Transient);
        services.Register<IPermissionRepository, PermissionRepository>(Lifestyle.Transient);
        services.Register<IPlayerRepository, PlayerRepository>();
        services.Register<IAuditRepository, AuditRepository>();

        return services;
    }

    public static ServicesBuilder AddEvoScMigrations(this ServicesBuilder services)
    {
        services.Register<IMigrationManager, MigrationManager>(Lifestyle.Scoped);
        return services;
    }
}
