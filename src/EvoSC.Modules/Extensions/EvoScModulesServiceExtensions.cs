using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Models;
using SimpleInjector;

namespace EvoSC.Modules.Extensions;

public static class EvoScModulesServiceExtensions
{
    /// <summary>
    /// Add the module system to the service container.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="knownInternalModuleIds">The ids of the modules the application was built to
    /// recognize as internal. A module cannot grant itself internal status just by declaring
    /// [Module(IsInternal = true)] unless its id is in this list.</param>
    /// <returns></returns>
    public static Container AddEvoScModules(this Container services, IEnumerable<string> knownInternalModuleIds)
    {
        services.RegisterInstance<IKnownInternalModules>(new KnownInternalModules(knownInternalModuleIds));
        services.RegisterSingleton<IModuleManager, ModuleManager>();
        return services;
    }
}
