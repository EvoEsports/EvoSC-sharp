using EvoSC.Common.Interfaces;
using SimpleInjector;

namespace EvoSC.Common.Events;

public static class EventManagerServiceExtensions
{
    public static Container AddEvoScEvents(this Container services)
    {
        services.RegisterSingleton<IEventManager, EventManager>();
        return services;
    }
}
