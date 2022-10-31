using EvoSC.Common.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;

namespace EvoSC.Common.Services;

public static class CommonServiceExtensions
{
    public static Container AddEvoScCommonServices(this Container services)
    {
        services.Register<IPlayerService, PlayerService>(Lifestyle.Transient);
        return services;
    }
}
