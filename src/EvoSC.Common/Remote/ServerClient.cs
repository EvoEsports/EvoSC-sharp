using EvoSC.Common.Config.Models;
using EvoSC.Common.Events;
using EvoSC.Common.Exceptions;
using EvoSC.Common.Interfaces;
using GbxRemoteNet;
using GbxRemoteNet.Events;
using Microsoft.Extensions.Logging;

namespace EvoSC.Common.Remote;

public class ServerClient : IServerClient
{
    private readonly GbxRemoteClient _gbxRemote;
    private readonly ServerConfig _config;
    private readonly ILogger<ServerClient> _logger;
    private readonly IEvoSCApplication _app;
    private GbxRemoteEventDispatcher _eventDispatcher;

    public GbxRemoteClient Remote => _gbxRemote;
    
    public ServerClient(ServerConfig config, ILogger<ServerClient> logger, IEvoSCApplication app, EventManager events)
    {
        _config = config;
        _logger = logger;
        _app = app;
        _gbxRemote = new GbxRemoteClient(config.Host, config.Port, logger);
        
        _gbxRemote.OnDisconnected += OnDisconnected;

        _eventDispatcher = new GbxRemoteEventDispatcher(_gbxRemote, events);
    }

    private async Task OnDisconnected()
    {
        await ConnectOrShutdown(_app.MainCancellationToken, true);
    }

    /// <summary>
    /// Try to set up the connection, authenticate and enable callbacks.
    /// </summary>
    /// <returns></returns>
    private async Task<bool> SetupConnection()
    {
        if (!await _gbxRemote.ConnectAsync())
        {
            return false;
        }

        if (!await _gbxRemote.AuthenticateAsync(_config.Username, _config.Password))
        {
            _logger.LogError("Failed to authenticate to the server");
            return false;
        }

        await _gbxRemote.EnableCallbackTypeAsync();

        return true;
    }

    /// <summary>
    /// Try to connect to the server. If it fails and there are no re-tries, the application
    /// is shut down.
    /// </summary>
    /// <param name="cancelToken"></param>
    /// <param name="disconnected"></param>
    /// <exception cref="Exception"></exception>
    private async Task ConnectOrShutdown(CancellationToken cancelToken, bool disconnected=false)
    {
        try
        {
            do
            {
                if (!disconnected || (disconnected && _config.RetryConnection))
                {
                    if (await SetupConnection())
                    {
                        return;
                    }
                }

                await Task.Delay(1000);
                
            } while (!cancelToken.IsCancellationRequested && _config.RetryConnection);

            throw new Exception();
        }
        catch (Exception e)
        {
            await _app.ShutdownAsync();
        }
    }
    
    public async Task StartAsync(CancellationToken token)
    {
        await ConnectOrShutdown(token);
    }

    public async Task StopAsync(CancellationToken token)
    {
        await _gbxRemote.DisconnectAsync();
    }
}
