using EvoSC.Common.Config.Models;
using EvoSC.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace EvoSC.Common.Remote;

public static class RemoteClientServiceExtensions
{
    public static IServiceCollection AddGbxRemoteClient(this IServiceCollection services)
    {
        services.AddSingleton<IServerClient, ServerClient>();
        
        return services;
    }
}
