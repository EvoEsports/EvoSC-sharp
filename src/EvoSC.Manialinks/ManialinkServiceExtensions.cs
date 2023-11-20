using EvoSC.Common.Interfaces.Middleware;
using EvoSC.Common.Middleware;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Manialinks.Middleware;
using SimpleInjector;

namespace EvoSC.Manialinks;

public static class ManialinkServiceExtensions
{
    public static Container AddEvoScManialinks(this Container services)
    {
        services.RegisterSingleton<IManialinkInteractionHandler, ManialinkInteractionHandler>();
        services.RegisterSingleton<IManialinkActionManager, ManialinkActionManager>();
        services.RegisterSingleton<IManialinkManager, ManialinkManager>();
        
        return services;
    }

    public static void UseEvoScManialinks(this IActionPipelineManager pipelineManager, Container services)
    {
        pipelineManager.UseMiddleware<ManialinkPermissionMiddleware>(PipelineType.ControllerAction, services);
    }
}
