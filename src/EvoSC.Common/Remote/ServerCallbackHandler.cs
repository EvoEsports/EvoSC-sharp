using EvoSC.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace EvoSC.Common.Remote;

public class ServerCallbackHandler : IServerCallbackHandler
{
    private readonly ILogger<ServerCallbackHandler> _logger;
    private readonly IServerClient _server;
    private readonly IEventManager _events;

    public ServerCallbackHandler(ILogger<ServerCallbackHandler> logger, IServerClient server, IEventManager events)
    {
        _logger = logger;
        _server = server;
        _events = events;
        
        SetupCallbacks();
    }

    private void SetupCallbacks()
    {
        _server.Remote.OnPlayerChat += (sender, e) => _events.Fire(GbxRemoteEvent.PlayerChat, e);
        _server.Remote.OnPlayerConnect += (sender, e) => _events.Fire(GbxRemoteEvent.PlayerConnect, e);
        _server.Remote.OnPlayerDisconnect += (sender, e) => _events.Fire(GbxRemoteEvent.PlayerDisconnect, e);
        _server.Remote.OnPlayerInfoChanged += (sender, e) => _events.Fire(GbxRemoteEvent.PlayerInfoChanged, e);
        _server.Remote.OnEndMap += (sender, e) => _events.Fire(GbxRemoteEvent.EndMap, e);
        _server.Remote.OnEndMatch += (sender, e) => _events.Fire(GbxRemoteEvent.EndMatch, e);
        _server.Remote.OnBeginMap += (sender, e) => _events.Fire(GbxRemoteEvent.BeginMap, e);
        _server.Remote.OnBeginMatch += (sender, e) => _events.Fire(GbxRemoteEvent.BeginMatch, e);
        _server.Remote.OnEcho += (sender, e) => _events.Fire(GbxRemoteEvent.Echo, e);
        _server.Remote.OnPlayerManialinkPageAnswer += (sender, e) => _events.Fire(GbxRemoteEvent.ManialinkPageAnswer, e);
        _server.Remote.OnMapListModified += (sender, e) => _events.Fire(GbxRemoteEvent.MapListModified, e);
    }
}
