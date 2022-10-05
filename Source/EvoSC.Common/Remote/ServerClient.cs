using EvoSC.Common.Config.Models;
using EvoSC.Common.Interfaces;
using GbxRemoteNet;
using Microsoft.Extensions.Logging;

namespace EvoSC.Common.Remote;

public class ServerClient : GbxRemoteClient
{
    public ServerClient(ServerConfig config, ILogger<ServerClient> logger) : base(config.Host, config.Port, logger)
    {
        
    }
}
