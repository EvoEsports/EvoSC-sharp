using System;
using EvoSC.Core.Configuration;
using GbxRemoteNet;

namespace EvoSC.Core;

public class ServerConnection
{
    public void ConnectToServer(ServerConnectionConfig _serverConnectionConfig)
    {
        var gbxRemoteClient = new GbxRemoteClient(_serverConnectionConfig.Host, _serverConnectionConfig.Port);
        
    }
}
