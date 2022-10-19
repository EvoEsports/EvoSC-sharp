using EvoSC.Common.Services;
using EvoSC.Modules.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;

namespace EvoSC.Modules.Extensions;

public static class EvoScModulesServiceExtensions
{
    public static Container AddEvoScModules(this Container services)
    {
        services.RegisterSingleton<IModuleServicesManager, ModuleServicesManager>();
        services.RegisterSingleton<IModuleManager, ModuleManager>();
        return services;
    }
}
