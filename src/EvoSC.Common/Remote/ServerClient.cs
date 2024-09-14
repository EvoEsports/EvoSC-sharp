using EvoSC.Common.Config.Models;
using EvoSC.Common.Exceptions;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Services;
using GbxRemoteNet;
using GbxRemoteNet.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EvoSC.Common.Remote;

public partial class ServerClient : IServerClient
{
    private readonly IGbxRemoteClient _gbxRemote;
    private readonly IEvoScBaseConfig _config;
    private readonly ILogger<ServerClient> _logger;
    private readonly IEvoSCApplication _app;
    private readonly IThemeManager _themes;

    private bool _connected;

    public IGbxRemoteClient Remote => _gbxRemote;
    public bool Connected => _connected;
    public IChatService Chat { get; }

    public ServerClient(IEvoScBaseConfig config, ILogger<ServerClient> logger, IEvoSCApplication app, IThemeManager themes)
    {
        _config = config;
        _logger = logger;
        _app = app;
        _themes = themes;
        
        _connected = false;
        _gbxRemote = new GbxRemoteClient(config.Server.Host, config.Server.Port, logger);
        
        _gbxRemote.OnDisconnected += OnDisconnectedAsync;

        Chat = new ChatService(this, themes);
    }

    private async Task OnDisconnectedAsync()
    {
        _connected = false;
        await ConnectOrShutdownAsync(_app.MainCancellationToken, true);
    }

    /// <summary>
    /// Try to set up the connection, authenticate and enable callbacks.
    /// </summary>
    /// <returns></returns>
    private async Task<bool> SetupConnectionAsync()
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
        await _gbxRemote.SendHideManialinkPageAsync(); //hide all manialinks on connect
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
    private async Task ConnectOrShutdownAsync(CancellationToken cancelToken, bool disconnected=false)
    {
        try
        {
            do
            {
                if (!disconnected || (disconnected && _config.Server.RetryConnection))
                {
                    if (await SetupConnectionAsync())
                    {
                        _connected = true;
                        return;
                    }
                }

                await Task.Delay(1000, cancelToken);

            } while (!cancelToken.IsCancellationRequested && _config.Server.RetryConnection);

            throw new ServerDisconnectedException();
        }
        catch (Exception ex)
        {
            if (ex is ServerDisconnectedException serverDisconnectEx)
            {
                _logger.LogError(serverDisconnectEx, "Either too many connection attempts were made or the client got disconnected from the server");
            }
            else
            {
                _logger.LogError(ex, "An error occured while trying to connect to the server");
            }
            
            await _app.ShutdownAsync();
        }
    }
    
    public async Task StartAsync(CancellationToken token)
    {
        await ConnectOrShutdownAsync(token);
    }

    public async Task StopAsync(CancellationToken token)
    {
        await _gbxRemote.ChatEnableManualRoutingAsync(false, false);
        await _gbxRemote.SendHideManialinkPageAsync();  //hide all manialinks on disconnect
        await _gbxRemote.DisconnectAsync();
    }

    public async Task<string> GetMapsDirectoryAsync()
    {
        var mapsDir = _config.Path.Maps;

        if (mapsDir == string.Empty)
        {
            mapsDir = await Remote.GetMapsDirectoryAsync();
        }

        // if it's still empty and doesn't exist, we should throw an error
        if (mapsDir == string.Empty && !Directory.Exists(mapsDir))
        {
            throw new DirectoryNotFoundException("Failed to find an existing maps directory.");
        }

        return mapsDir;
    }

    public async Task<bool> FileExistsAsync(string file)
    {
        var mapsDir = await GetMapsDirectoryAsync();

        return File.Exists(Path.Combine(mapsDir, file));
    }
}
