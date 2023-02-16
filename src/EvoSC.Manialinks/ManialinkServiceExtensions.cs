using EvoSC.Manialinks.Interfaces;
using SimpleInjector;

namespace EvoSC.Manialinks;

public static class ManialinkServiceExtensions
{
    public static Container AddEvoScManialinks(this Container services)
    {
        services.RegisterSingleton<IManialinkInteractionHandler, ManialinkInteractionHandler>();
        services.RegisterSingleton<IManialinkActionManager, ManialinkActionManager>();
        
        return services;
    }
}
