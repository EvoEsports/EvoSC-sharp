using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Permissions;
using Microsoft.Extensions.DependencyInjection;
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
        services.Register<IPlayerService, PlayerService>(Lifestyle.Transient);
        services.Register<IPermissionManager, PermissionManager>(Lifestyle.Transient);
        
        return services;
    }
}
