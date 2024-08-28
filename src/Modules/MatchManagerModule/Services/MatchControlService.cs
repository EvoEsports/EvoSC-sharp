using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.MatchManagerModule.Events;
using EvoSC.Modules.Official.MatchManagerModule.Interfaces;

namespace EvoSC.Modules.Official.MatchManagerModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class MatchControlService(IServerClient server, IEventManager events) : IMatchControlService
{
    private readonly Dictionary<PlayerTeam, int> _matchPoints = [];
    private readonly Dictionary<PlayerTeam, int> _mapPoints = [];

    public async Task StartMatchAsync()
    {
        await RestartMatchAsync();

        await events.RaiseAsync(FlowControlEvent.MatchStarted, EventArgs.Empty);
    }

    public async Task EndMatchAsync()
    {
        await events.RaiseAsync(FlowControlEvent.MatchEnded, EventArgs.Empty);
    }

    public async Task EndRoundAsync()
    {
        await server.Remote.TriggerModeScriptEventArrayAsync("Trackmania.ForceEndRound");
        await server.Remote.TriggerModeScriptEventArrayAsync("Trackmania.WarmUp.ForceStopRound");

        await events.RaiseAsync(FlowControlEvent.ForcedRoundEnd, EventArgs.Empty);
    }

    public async Task RestartMatchAsync()
    {
        await server.Remote.RestartMapAsync();

        await events.RaiseAsync(FlowControlEvent.MatchRestarted, EventArgs.Empty);
    }

    public async Task SkipMapAsync()
    {
        await server.Remote.NextMapAsync();

        await events.RaiseAsync(FlowControlEvent.MapSkipped, EventArgs.Empty);
    }

    public async Task SetTeamPointsAsync(PlayerTeam team, int points)
    {
        await server.Remote.TriggerModeScriptEventArrayAsync("Trackmania.SetTeamPoints", ((int)team).ToString(),
            points.ToString(), points.ToString(), points.ToString());
        
        _mapPoints[team] = points;
        _matchPoints[team] = points;
    }
        

    public Task SetTeamRoundPointsAsync(PlayerTeam team, int points) =>
        server.Remote.TriggerModeScriptEventArrayAsync("Trackmania.SetTeamPoints", ((int)team).ToString(),
            points.ToString(), "", "");

    public async Task SetTeamMapPointsAsync(PlayerTeam team, int points)
    {
        await server.Remote.TriggerModeScriptEventArrayAsync("Trackmania.SetTeamPoints", ((int)team).ToString(), "",
            points.ToString(), _matchPoints.GetValueOrDefault(team).ToString());

        _mapPoints[team] = points;
    }

    public async Task SetTeamMatchPointsAsync(PlayerTeam team, int points)
    {
        await server.Remote.TriggerModeScriptEventArrayAsync("Trackmania.SetTeamPoints", ((int)team).ToString(), "",
            _mapPoints.GetValueOrDefault(team).ToString(), points.ToString());

        _matchPoints[team] = points;
    }

    public Task PauseMatchAsync() =>
        server.Remote.TriggerModeScriptEventArrayAsync("Maniaplanet.Pause.SetActive", "true");

    public Task UnpauseMatchAsync() =>
        server.Remote.TriggerModeScriptEventArrayAsync("Maniaplanet.Pause.SetActive", "false");

    public Task RequestScoresAsync() =>
        server.Remote.TriggerModeScriptEventArrayAsync("Trackmania.GetScores");

    public Task UpdateTeamPointsAsync(int team1MapPoints, int team1MatchPoints, int team2MapPoints,
        int team2MatchPoints)
    {
        _mapPoints[PlayerTeam.Team1] = team1MapPoints;
        _matchPoints[PlayerTeam.Team1] = team1MatchPoints;
        _mapPoints[PlayerTeam.Team2] = team2MapPoints;
        _matchPoints[PlayerTeam.Team2] = team2MatchPoints;

        return Task.CompletedTask;
    }
}
