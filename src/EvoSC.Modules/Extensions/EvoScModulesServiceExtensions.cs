using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;

namespace EvoSC.Modules.Extensions;

public static class EvoScModulesServiceExtensions
{
    public static Container AddEvoScModules(this Container services)
    {
        services.RegisterSingleton<IModuleManager, ModuleManager>();
        return services;
    }
}
