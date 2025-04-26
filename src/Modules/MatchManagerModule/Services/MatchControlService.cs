using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Models.Callbacks;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.MatchManagerModule.Events;
using EvoSC.Modules.Official.MatchManagerModule.Interfaces;

namespace EvoSC.Modules.Official.MatchManagerModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class MatchControlService(IServerClient server, IEventManager events)
    : IMatchControlService
{
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

    public Task SetTeamPointsAsync(PlayerTeam team, int points) =>
        UpdateTeamScoreAsync(team, points, points, points);

    public async Task SetTeamRoundPointsAsync(PlayerTeam team, int newRoundPoints)
    {
        var current = await GetTeamScoreAsync(team);

        await UpdateTeamScoreAsync(team, newRoundPoints, current.MapPoints, current.MatchPoints);
    }

    public async Task SetTeamMapPointsAsync(PlayerTeam team, int newMapPoints)
    {
        var current = await GetTeamScoreAsync(team);

        await UpdateTeamScoreAsync(team, current.RoundPoints, newMapPoints, current.MatchPoints);
    }

    public async Task SetTeamMatchPointsAsync(PlayerTeam team, int newMatchPoints)
    {
        var current = await GetTeamScoreAsync(team);

        await UpdateTeamScoreAsync(team, current.RoundPoints, current.MapPoints, newMatchPoints);
    }

    public Task UpdateTeamScoreAsync(PlayerTeam team, int roundPoints, int mapPoints, int matchPoints) =>
        server.Remote.TriggerModeScriptEventArrayAsync("Trackmania.SetTeamPoints", ((int)team).ToString(),
            roundPoints.ToString(), mapPoints.ToString(), matchPoints.ToString());

    public async Task<TeamScore> GetTeamScoreAsync(PlayerTeam team)
    {
        var (getScoresResponse, _) = await server.Remote.GetModeScriptResponseAsync("Trackmania.GetScores");

        if (getScoresResponse == null)
        {
            return new TeamScore();
        }

        var teamScores = getScoresResponse.GetValue("teams", StringComparison.Ordinal)?.ToObject<TeamScore[]>();

        return teamScores != null ? teamScores[(int)team] : new TeamScore();
    }

    public Task PauseMatchAsync() =>
        server.Remote.TriggerModeScriptEventArrayAsync("Maniaplanet.Pause.SetActive", "true");

    public Task UnpauseMatchAsync() =>
        server.Remote.TriggerModeScriptEventArrayAsync("Maniaplanet.Pause.SetActive", "false");

    public Task RequestScoresAsync() =>
        server.Remote.TriggerModeScriptEventArrayAsync("Trackmania.GetScores");
}
