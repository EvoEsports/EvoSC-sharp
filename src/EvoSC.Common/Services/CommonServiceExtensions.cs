using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Services;
using SimpleInjector;

namespace EvoSC.Common.Services;

public static class CommonServiceExtensions
{
    /// <summary>
    /// Add the common core services to the service container.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static Container AddEvoScCommonServices(this Container services)
    {
        services.Register<IPlayerManagerService, PlayerManagerService>(Lifestyle.Transient);
        services.Register<IMapService, MapService>(Lifestyle.Transient);
        services.RegisterSingleton<IPlayerCacheService, PlayerCacheService>();
        services.Register<IMatchSettingsService, MatchSettingsService>(Lifestyle.Transient);
        services.Register<IAuditService, AuditService>(Lifestyle.Transient);
        services.RegisterSingleton<IServiceContainerManager, ServiceContainerManager>();
        
        return services;
    }

    public static Container AddEvoScCommonScopedServices(this Container services)
    {
        services.Register<IContextService, ContextService>(Lifestyle.Scoped);
        
        return services;
    }
}
