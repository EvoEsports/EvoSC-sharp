using EvoSC.Common.Interfaces;
using EvoSC.Common.Models.Callbacks;
using EvoSC.Common.Remote.EventArgsModels;
using Newtonsoft.Json.Linq;

namespace EvoSC.Common.Remote;

public class ServerCallbackHandler : IServerCallbackHandler
{
    private readonly IServerClient _server;
    private readonly IEventManager _events;

    public ServerCallbackHandler(IServerClient server, IEventManager events)
    {
        _server = server;
        _events = events;
        
        SetupCallbacks();
    }

    private void SetupCallbacks()
    {
        _server.Remote.OnPlayerChat += (sender, e) => _events.RaiseAsync(GbxRemoteEvent.PlayerChat, e, sender);
        _server.Remote.OnPlayerConnect += (sender, e) => _events.RaiseAsync(GbxRemoteEvent.PlayerConnect, e, sender);
        _server.Remote.OnPlayerDisconnect += (sender, e) => _events.RaiseAsync(GbxRemoteEvent.PlayerDisconnect, e, sender);
        _server.Remote.OnPlayerInfoChanged += (sender, e) => _events.RaiseAsync(GbxRemoteEvent.PlayerInfoChanged, e, sender);
        _server.Remote.OnEndMap += (sender, e) => _events.RaiseAsync(GbxRemoteEvent.EndMap, e, sender);
        _server.Remote.OnEndMatch += (sender, e) => _events.RaiseAsync(GbxRemoteEvent.EndMatch, e, sender);
        _server.Remote.OnBeginMap += (sender, e) => _events.RaiseAsync(GbxRemoteEvent.BeginMap, e, sender);
        _server.Remote.OnBeginMatch += (sender, e) => _events.RaiseAsync(GbxRemoteEvent.BeginMatch, e, sender);
        _server.Remote.OnEcho += (sender, e) => _events.RaiseAsync(GbxRemoteEvent.Echo, e, sender);
        _server.Remote.OnPlayerManialinkPageAnswer += (sender, e) => _events.RaiseAsync(GbxRemoteEvent.ManialinkPageAnswer, e, sender);
        _server.Remote.OnMapListModified += (sender, e) => _events.RaiseAsync(GbxRemoteEvent.MapListModified, e, sender);
        _server.Remote.OnStatusChanged += (sender, e) => _events.RaiseAsync(GbxRemoteEvent.StatusChanged, e, sender);
        _server.Remote.OnModeScriptCallback += OnModeScriptCallbackAsync;
    }

    private async Task OnModeScriptCallbackAsync(string method, JObject data)
    {
        switch (method)
        {
            case "Trackmania.Scores":
                await _events.RaiseAsync(ModeScriptEvent.Scores,
                    new ScoresEventArgs
                    {
                        Section = data.GetValue("section", StringComparison.Ordinal).ToObject<string>(),
                        UseTeams = data.GetValue("useteams", StringComparison.Ordinal).ToObject<bool>(),
                        WinnerTeam = data.GetValue("winnerteam", StringComparison.Ordinal).ToObject<int>(),
                        WinnerPlayer = data.GetValue("winnerplayer", StringComparison.Ordinal).ToObject<string>(),
                        Teams = data.GetValue("teams", StringComparison.Ordinal).ToObject<TeamScore[]>(),
                        Players = data.GetValue("players", StringComparison.Ordinal).ToObject<PlayerScore[]>()
                    });
                break;
            case "Trackmania.Event.GiveUp":
                await _events.RaiseAsync(ModeScriptEvent.GiveUp,
                    new PlayerUpdateEventArgs
                    {
                        Time = data.GetValue("time", StringComparison.Ordinal).ToObject<int>(),
                        Login = data.GetValue("login", StringComparison.Ordinal).ToObject<string>(),
                        AccountId = data.GetValue("accountid", StringComparison.Ordinal).ToObject<string>()
                    });
                break;
            case "Trackmania.Event.WayPoint":
                await _events.RaiseAsync(ModeScriptEvent.WayPoint,
                    new WayPointEventArgs
                    {
                        Time = data.GetValue("time", StringComparison.Ordinal).ToObject<int>(),
                        Login = data.GetValue("login", StringComparison.Ordinal).ToObject<string>(),
                        AccountId = data.GetValue("accountid", StringComparison.Ordinal).ToObject<string>(),
                        RaceTime = data.GetValue("racetime", StringComparison.Ordinal).ToObject<int>(),
                        LapTime = data.GetValue("laptime", StringComparison.Ordinal).ToObject<int>(),
                        CheckpointInRace = data.GetValue("checkpointinrace", StringComparison.Ordinal).ToObject<int>(),
                        CheckpointInLap = data.GetValue("checkpointinlap", StringComparison.Ordinal).ToObject<int>(),
                        IsEndRace = data.GetValue("isendrace", StringComparison.Ordinal).ToObject<bool>(),
                        IsEndLap = data.GetValue("isendlap", StringComparison.Ordinal).ToObject<bool>(),
                        CurrentRaceCheckpoints = data.GetValue("curracecheckpoints", StringComparison.Ordinal).ToObject<int[]>(),
                        CurrentLapCheckpoints = data.GetValue("curlapcheckpoints", StringComparison.Ordinal).ToObject<int[]>(),
                        BlockId = data.GetValue("blockid", StringComparison.Ordinal).ToObject<string>(),
                        Speed = data.GetValue("speed", StringComparison.Ordinal).ToObject<int>()
                    });
                break;
            case "Trackmania.Event.Respawn":
                await _events.RaiseAsync(ModeScriptEvent.Respawn,
                    new RespawnEventArgs
                    {
                        Time = data.GetValue("time", StringComparison.Ordinal).ToObject<int>(),
                        Login = data.GetValue("login", StringComparison.Ordinal).ToObject<string>(),
                        AccountId = data.GetValue("accountid", StringComparison.Ordinal).ToObject<string>(),
                        NbRespawns = data.GetValue("nbrespawns", StringComparison.Ordinal).ToObject<int>(),
                        RaceTime = data.GetValue("racetime", StringComparison.Ordinal).ToObject<int>(),
                        LapTime = data.GetValue("laptime", StringComparison.Ordinal).ToObject<int>(),
                        CheckpointInRace = data.GetValue("checkpointinrace", StringComparison.Ordinal).ToObject<int>(),
                        CheckpointInLap = data.GetValue("checkpointinlap", StringComparison.Ordinal).ToObject<int>(),
                        Speed = data.GetValue("speed", StringComparison.Ordinal).ToObject<int>()
                    });
                break;
            case "Trackmania.Event.StartLine":
                await _events.RaiseAsync(ModeScriptEvent.StartLine,
                    new PlayerUpdateEventArgs
                    {
                        Time = data.GetValue("time", StringComparison.Ordinal).ToObject<int>(),
                        Login = data.GetValue("login", StringComparison.Ordinal).ToObject<string>(),
                        AccountId = data.GetValue("accountid", StringComparison.Ordinal).ToObject<string>()
                    });
                break;
            case "Trackmania.WarmUp.End":
                await _events.RaiseAsync(ModeScriptEvent.WarmUpEnd, EventArgs.Empty);
                break;
            case "Trackmania.WarmUp.Start":
                await _events.RaiseAsync(ModeScriptEvent.WarmUpStart, EventArgs.Empty);
                break;
            case "Trackmania.Event.Eliminated":
                await _events.RaiseAsync(ModeScriptEvent.Eliminated,
                    new PlayerUpdateEventArgs
                    {
                        Time = data.GetValue("time", StringComparison.Ordinal).ToObject<int>(),
                        Login = data.GetValue("login", StringComparison.Ordinal).ToObject<string>(),
                        AccountId = data.GetValue("accountid", StringComparison.Ordinal).ToObject<string>()
                    });
                break;
            case "Maniaplanet.Podium_Start":
                await _events.RaiseAsync(ModeScriptEvent.PodiumStart,
                    new PodiumEventArgs { Time = data.GetValue("time", StringComparison.Ordinal).ToObject<int>() });
                    break;
            case "Maniaplanet.Podium_End":
                await _events.RaiseAsync(ModeScriptEvent.PodiumEnd,
                    new PodiumEventArgs { Time = data.GetValue("time", StringComparison.Ordinal).ToObject<int>() });
                break;
        }
        
        await _events.RaiseAsync(ModeScriptEvent.Any, new ModeScriptEventArgs {Method = method, Args = data});
    }
}
