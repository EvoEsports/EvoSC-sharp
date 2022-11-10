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
        _server.Remote.OnPlayerChat += (sender, e) => _events.Raise(GbxRemoteEvent.PlayerChat, e, sender);
        _server.Remote.OnPlayerConnect += (sender, e) => _events.Raise(GbxRemoteEvent.PlayerConnect, e, sender);
        _server.Remote.OnPlayerDisconnect += (sender, e) => _events.Raise(GbxRemoteEvent.PlayerDisconnect, e, sender);
        _server.Remote.OnPlayerInfoChanged += (sender, e) => _events.Raise(GbxRemoteEvent.PlayerInfoChanged, e, sender);
        _server.Remote.OnEndMap += (sender, e) => _events.Raise(GbxRemoteEvent.EndMap, e, sender);
        _server.Remote.OnEndMatch += (sender, e) => _events.Raise(GbxRemoteEvent.EndMatch, e, sender);
        _server.Remote.OnBeginMap += (sender, e) => _events.Raise(GbxRemoteEvent.BeginMap, e, sender);
        _server.Remote.OnBeginMatch += (sender, e) => _events.Raise(GbxRemoteEvent.BeginMatch, e, sender);
        _server.Remote.OnEcho += (sender, e) => _events.Raise(GbxRemoteEvent.Echo, e, sender);
        _server.Remote.OnPlayerManialinkPageAnswer += (sender, e) => _events.Raise(GbxRemoteEvent.ManialinkPageAnswer, e, sender);
        _server.Remote.OnMapListModified += (sender, e) => _events.Raise(GbxRemoteEvent.MapListModified, e, sender);
    }
}
