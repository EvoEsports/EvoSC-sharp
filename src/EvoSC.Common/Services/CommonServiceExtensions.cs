using EvoSC.Common.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EvoSC.Common.Services;

public static class CommonServiceExtensions
{
    public static IServiceCollection AddEvoScCommonServices(this IServiceCollection services)
    {
        return services.AddTransient<IPlayerService, PlayerService>();
    }
}
