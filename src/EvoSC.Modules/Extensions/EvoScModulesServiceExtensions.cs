using EvoSC.Modules.Interfaces;
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
        services.RegisterSingleton<IModuleManager, ModuleManager>();
        return services;
    }
}
