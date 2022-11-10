using EvoSC.Common.Services;
using EvoSC.Modules.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;

namespace EvoSC.Modules.Extensions;

public static class EvoScModulesServiceExtensions
{
    /// <summary>
    /// Add the module system to the service container.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static Container AddEvoScModules(this Container services)
    {
        services.RegisterSingleton<IModuleServicesManager, ModuleServicesManager>();
        services.RegisterSingleton<IModuleManager, ModuleManager>();
        return services;
    }
}
