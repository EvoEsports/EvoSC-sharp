using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using EvoSC.Core.Configuration;
using EvoSC.Interfaces;
using EvoSC.Interfaces.Players;
using GbxRemoteNet;
using Newtonsoft.Json.Linq;
using NLog;

namespace EvoSC.Core;

public class ServerConnection
{
    private readonly GbxRemoteClient _gbxRemoteClient;
    private readonly IEnumerable<IGbxEventHandler> _eventHandlers;
    private readonly IPlayerService _playerService;
    private readonly Logger _logger = LogManager.GetCurrentClassLogger();

    public ServerConnection(GbxRemoteClient gbxRemoteClient, IEnumerable<IGbxEventHandler> eventHandlers, IPlayerService playerService)
    {
        _gbxRemoteClient = gbxRemoteClient;
        _eventHandlers = eventHandlers;
        _playerService = playerService;
    }

    public async Task ConnectToServer(Dedicated config)
    {
        var connected = await _gbxRemoteClient.ConnectAsync();
        if (!connected)
        {
            Console.WriteLine(await _gbxRemoteClient.GetLastConnectionErrorMessageAsync());
        }
        await _gbxRemoteClient.SetApiVersionAsync("2022-03-21");
        var authenticated = await _gbxRemoteClient.AuthenticateAsync(config.AdminLogin, config.AdminPassword);

        if (!authenticated)
        {
            throw new AuthenticationException("Could not authenticate to server - login or password is incorrect!");
        }

        await _gbxRemoteClient.EnableCallbackTypeAsync();

        await _playerService.AddConnectedPlayers();
    }

    public void InitializeEventHandlers()
    {
        foreach (var eventHandler in _eventHandlers)
        {
            eventHandler.HandleEvents(_gbxRemoteClient);
        }
    }
}
