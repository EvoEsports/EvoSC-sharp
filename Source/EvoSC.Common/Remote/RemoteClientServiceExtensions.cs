using EvoSC.Common.Config.Models;
using Microsoft.Extensions.DependencyInjection;

namespace EvoSC.Common.Remote;

public static class RemoteClientServiceExtensions
{
    public static IServiceCollection AddGbxRemoteClient(this IServiceCollection services, ServerConfig config)
    {
        return services;
    }
}
