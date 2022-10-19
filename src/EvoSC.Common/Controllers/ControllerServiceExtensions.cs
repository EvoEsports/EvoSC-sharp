using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;

namespace EvoSC.Common.Controllers;

public static class ControllerServiceExtensions
{
    public static Container AddEvoScControllers(this Container services)
    {
        // return services.AddSingleton<IControllerManager, ControllerManager>();
        services.RegisterSingleton<IControllerManager, ControllerManager>();
        return services;
    }
}
