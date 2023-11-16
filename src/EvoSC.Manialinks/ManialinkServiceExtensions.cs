using EvoSC.Common.Interfaces.Middleware;
using EvoSC.Common.Middleware;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Manialinks.Interfaces.Themes;
using EvoSC.Manialinks.Middleware;
using EvoSC.Manialinks.Themes;
using SimpleInjector;

namespace EvoSC.Manialinks;

public static class ManialinkServiceExtensions
{
    public static Container AddEvoScManialinks(this Container services)
    {
        services.RegisterSingleton<IManialinkInteractionHandler, ManialinkInteractionHandler>();
        services.RegisterSingleton<IManialinkActionManager, ManialinkActionManager>();
        services.RegisterSingleton<IManialinkManager, ManialinkManager>();
        services.RegisterSingleton<IThemeManager, ThemeManager>();
        
        return services;
    }

    public static void UseEvoScManialinks(this IActionPipelineManager pipelineManager, Container services)
    {
        pipelineManager.UseMiddleware<ManialinkPermissionMiddleware>(PipelineType.ControllerAction, services);
    }
}
