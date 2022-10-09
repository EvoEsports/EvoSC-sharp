using Microsoft.Extensions.DependencyInjection;

namespace EvoSC.Modules.Extensions;

public static class EvoScModulesServiceExtensions
{
    public static IServiceCollection AddEvoScModules(this IServiceCollection services)
    {
        services.AddSingleton<IModuleManager, ModuleManager>();
        
        return services;
    }
}
