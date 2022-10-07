using Microsoft.Extensions.DependencyInjection;

namespace EvoSC.Common.Events;

public static class EventManagerServiceExtensions
{
    public static IServiceCollection AddEvoScEvents(this IServiceCollection services)
    {
        services.AddSingleton<EventManager>();
        return services;
    }
}
