using EvoSC.Common.Config.Models;
using EvoSC.Common.Exceptions;
using EvoSC.Common.Interfaces;
using GbxRemoteNet;
using Microsoft.Extensions.Logging;

namespace EvoSC.Common.Remote;

public class ServerClient : IServerClient
{
    private readonly GbxRemoteClient _gbxRemote;
    private readonly ServerConfig _config;
    private readonly ILogger<ServerClient> _logger;

    public GbxRemoteClient Remote => _gbxRemote;
    
    public ServerClient(ServerConfig config, ILogger<ServerClient> logger)
    {
        _config = config;
        _logger = logger;
        _gbxRemote = new GbxRemoteClient(config.Host, config.Port, logger);
    }
    
    public async Task StartAsync(CancellationToken token)
    {
        if (!await _gbxRemote.AuthenticateAsync(_config.Username, _config.Password))
        {
            throw new GbxRemoteAuthenticationException();
        }

        _logger.LogDebug("Authenticated to the remote server as: {Username}", _config.Username);
    }

    public Task StopAsync(CancellationToken token)
    {
        throw new NotImplementedException();
    }
}
