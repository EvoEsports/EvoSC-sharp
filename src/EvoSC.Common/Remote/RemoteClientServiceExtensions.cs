using EvoSC.Common.Config.Models;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Remote.ChatRouter;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;

namespace EvoSC.Common.Remote;

public static class RemoteClientServiceExtensions
{
    /// <summary>
    /// Add the GbxRemote client to the service container.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static Container AddGbxRemoteClient(this Container services)
    {
        services.RegisterSingleton<IServerClient, ServerClient>();
        services.RegisterSingleton<IServerCallbackHandler, ServerCallbackHandler>();
        services.RegisterSingleton<IRemoteChatRouter, RemoteChatRouter>();
        
        return services;
    }
}
