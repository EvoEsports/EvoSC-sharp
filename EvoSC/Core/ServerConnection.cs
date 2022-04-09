using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using EvoSC.Core.Configuration;
using EvoSC.Interfaces;
using GbxRemoteNet;

namespace EvoSC.Core;

public class ServerConnection
{
    private readonly GbxRemoteClient _gbxRemoteClient;
    private readonly IEnumerable<IGbxEventHandler> _eventHandlers;
    
    public ServerConnection(GbxRemoteClient gbxRemoteClient, IEnumerable<IGbxEventHandler> eventHandlers)
    {
        _gbxRemoteClient = gbxRemoteClient;
        _eventHandlers = eventHandlers;
    }
    
    public void InitializeEventHandlers()
    {
        foreach (var eventHandler in _eventHandlers)
        {
            eventHandler.HandleEvents(_gbxRemoteClient);
        }
    }

    public async Task<bool> Authenticate(ServerConnectionConfig config)
    {
        return await _gbxRemoteClient.AuthenticateAsync(config.AdminLogin, config.AdminPassword);
    }
}
