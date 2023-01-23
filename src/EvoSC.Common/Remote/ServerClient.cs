using EvoSC.Common.Config.Models;
using EvoSC.Common.Interfaces;
using GbxRemoteNet;
using Microsoft.Extensions.Logging;

namespace EvoSC.Common.Remote;

public partial class ServerClient : IServerClient
{
    private readonly GbxRemoteClient _gbxRemote;
    private readonly IEvoScBaseConfig _config;
    private readonly ILogger<ServerClient> _logger;
    private readonly IEvoSCApplication _app;

    private bool _connected;

    public GbxRemoteClient Remote => _gbxRemote;
    public bool Connected => _connected;

    public ServerClient(IEvoScBaseConfig config, ILogger<ServerClient> logger, IEvoSCApplication app)
    {
        _config = config;
        _logger = logger;
        _app = app;
        _connected = false;
        _gbxRemote = new GbxRemoteClient(config.Server.Host, config.Server.Port, logger);
        
        _gbxRemote.OnDisconnected += OnDisconnected;
    }

    private async Task OnDisconnected()
    {
        _connected = false;
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

        if (!await _gbxRemote.AuthenticateAsync(_config.Server.Username, _config.Server.Password))
        {
            _logger.LogError("Failed to authenticate to the server");
            return false;
        }

        await _gbxRemote.SetApiVersionAsync(GbxRemoteClient.DefaultApiVersion);
        await _gbxRemote.EnableCallbackTypeAsync();
        await _gbxRemote.ChatEnableManualRoutingAsync();

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
                if (!disconnected || (disconnected && _config.Server.RetryConnection))
                {
                    if (await SetupConnection())
                    {
                        _connected = true;
                        return;
                    }
                }

                await Task.Delay(1000);
                
            } while (!cancelToken.IsCancellationRequested && _config.Server.RetryConnection);

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
        await _gbxRemote.ChatEnableManualRoutingAsync(false, false);
        await _gbxRemote.DisconnectAsync();
    }
}
