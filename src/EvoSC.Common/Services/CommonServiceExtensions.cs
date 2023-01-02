using EvoSC.Common.Database.Repository.Permissions;
using EvoSC.Common.Interfaces.Repository;
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
        services.Register<IPermissionRepository, PermissionRepository>(Lifestyle.Transient);
        
        return services;
    }
}
