using EvoSC.Common.Interfaces;
using EvoSC.Common.Models.Callbacks;
using EvoSC.Common.Remote.EventArgsModels;
using GbxRemoteNet.Events;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

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
        
        _server.Remote.OnModeScriptCallback += OnModeScriptCallback;
    }

    private async Task OnModeScriptCallback(string method, JObject data)
    {
        switch (method)
        {
            case "Trackmania.Scores":
                await _events.Raise(ModeScriptEvent.Scores,
                    new ScoresEventArgs
                    {
                        Section = data.GetValue("section").ToObject<string>(),
                        UseTeams = data.GetValue("useteams").ToObject<bool>(),
                        WinnerTeam = data.GetValue("winnerteam").ToObject<int>(),
                        WinnerPlayer = data.GetValue("winnerplayer").ToObject<string>(),
                        Teams = data.GetValue("teams").ToObject<TeamScore[]>(),
                        Players = data.GetValue("players").ToObject<PlayerScore[]>()
                    });
                break;
            case "Trackmania.Event.GiveUp":
                await _events.Raise(ModeScriptEvent.GiveUp,
                    new PlayerUpdateEventArgs
                    {
                        Time = data.GetValue("time").ToObject<int>(),
                        Login = data.GetValue("login").ToObject<string>(),
                        AccountId = data.GetValue("accountid").ToObject<string>()
                    });
                break;
            case "Trackmania.Event.WayPoint":
                await _events.Raise(ModeScriptEvent.WayPoint,
                    new WayPointEventArgs
                    {
                        Time = data.GetValue("time").ToObject<int>(),
                        Login = data.GetValue("login").ToObject<string>(),
                        AccountId = data.GetValue("accountid").ToObject<string>(),
                        RaceTime = data.GetValue("racetime").ToObject<int>(),
                        LapTime = data.GetValue("laptime").ToObject<int>(),
                        CheckpointInRace = data.GetValue("checkpointinrace").ToObject<int>(),
                        CheckpointInLap = data.GetValue("checkpointinlap").ToObject<int>(),
                        IsEndRace = data.GetValue("isendrace").ToObject<bool>(),
                        IsEndLap = data.GetValue("isendlap").ToObject<bool>(),
                        CurrentRaceCheckpoints = data.GetValue("curracecheckpoints").ToObject<int[]>(),
                        CurrentLapCheckpoints = data.GetValue("curlapcheckpoints").ToObject<int[]>(),
                        BlockId = data.GetValue("blockid").ToObject<string>(),
                        Speed = data.GetValue("speed").ToObject<int>()
                    });
                break;
            case "Trackmania.Event.Respawn":
                await _events.Raise(ModeScriptEvent.Respawn,
                    new RespawnEventArgs
                    {
                        Time = data.GetValue("time").ToObject<int>(),
                        Login = data.GetValue("login").ToObject<string>(),
                        AccountId = data.GetValue("accountid").ToObject<string>(),
                        NbRespawns = data.GetValue("nbrespawns").ToObject<int>(),
                        RaceTime = data.GetValue("racetime").ToObject<int>(),
                        LapTime = data.GetValue("laptime").ToObject<int>(),
                        CheckpointInRace = data.GetValue("checkpointinrace").ToObject<int>(),
                        CheckpointInLap = data.GetValue("checkpointinlap").ToObject<int>(),
                        Speed = data.GetValue("speed").ToObject<int>()
                    });
                break;
            case "Trackmania.Event.StartLine":
                await _events.Raise(ModeScriptEvent.StartLine,
                    new PlayerUpdateEventArgs
                    {
                        Time = data.GetValue("time").ToObject<int>(),
                        Login = data.GetValue("login").ToObject<string>(),
                        AccountId = data.GetValue("accountid").ToObject<string>()
                    });
                break;
            case "Trackmania.WarmUp.End":
                await _events.Raise(ModeScriptEvent.WarmUpEnd, EventArgs.Empty);
                break;
            case "Trackmania.WarmUp.Start":
                await _events.Raise(ModeScriptEvent.WarmUpStart, EventArgs.Empty);
                break;
            case "Trackmania.Event.Eliminated":
                await _events.Raise(ModeScriptEvent.Eliminated,
                    new PlayerUpdateEventArgs
                    {
                        Time = data.GetValue("time").ToObject<int>(),
                        Login = data.GetValue("login").ToObject<string>(),
                        AccountId = data.GetValue("accountid").ToObject<string>()
                    });
                break;
        }
        
        await _events.Raise(ModeScriptEvent.Any, new ModeScriptEventArgs {Method = method, Args = data});
    }
}
