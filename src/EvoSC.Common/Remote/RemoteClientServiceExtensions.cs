using EvoSC.Common.Config.Models;
using EvoSC.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;

namespace EvoSC.Common.Remote;

public static class RemoteClientServiceExtensions
{
    public static Container AddGbxRemoteClient(this Container services)
    {
        services.RegisterSingleton<IServerClient, ServerClient>();
        services.RegisterSingleton<IServerCallbackHandler, ServerCallbackHandler>();
        
        return services;
    }
}
