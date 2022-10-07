using EvoSC.Common.Events;
using GbxRemoteNet;
using GbxRemoteNet.Events;

namespace EvoSC.Common.Remote;

public class GbxRemoteEventDispatcher
{
    private readonly GbxRemoteClient _gbxRemote;
    private readonly EventManager _events;
    
    public GbxRemoteEventDispatcher(GbxRemoteClient gbxRemote, EventManager events)
    {
        _gbxRemote = gbxRemote;
        _events = events;
        
        SetupEvents();
    }

    private void SetupEvents()
    {
        _gbxRemote.OnPlayerChat += (sender, e) => _events.Fire(GbxRemoteEvent.PlayerChat, e);
        _gbxRemote.OnPlayerConnect += (sender, e) => _events.Fire(GbxRemoteEvent.PlayerConnect, e);
        _gbxRemote.OnPlayerDisconnect += (sender, e) => _events.Fire(GbxRemoteEvent.PlayerDisconnect, e);
        _gbxRemote.OnPlayerInfoChanged += (sender, e) => _events.Fire(GbxRemoteEvent.PlayerInfoChanged, e);
        _gbxRemote.OnEndMap += (sender, e) => _events.Fire(GbxRemoteEvent.EndMap, e);
        _gbxRemote.OnEndMatch += (sender, e) => _events.Fire(GbxRemoteEvent.EndMatch, e);
        _gbxRemote.OnBeginMap += (sender, e) => _events.Fire(GbxRemoteEvent.BeginMap, e);
        _gbxRemote.OnBeginMatch += (sender, e) => _events.Fire(GbxRemoteEvent.BeginMatch, e);
        _gbxRemote.OnEcho += (sender, e) => _events.Fire(GbxRemoteEvent.Echo, e);
        _gbxRemote.OnPlayerManialinkPageAnswer += (sender, e) => _events.Fire(GbxRemoteEvent.ManialinkPageAnswer, e);
        _gbxRemote.OnMapListModified += (sender, e) => _events.Fire(GbxRemoteEvent.MapListModified, e);
    }
}
