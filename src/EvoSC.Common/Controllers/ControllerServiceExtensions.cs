using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using Microsoft.Extensions.DependencyInjection;

namespace EvoSC.Common.Controllers;

public static class ControllerServiceExtensions
{
    public static IServiceCollection AddEvoScControllers(this IServiceCollection services)
    {
        return services.AddSingleton<IControllerManager, ControllerManager>();
    }
}
