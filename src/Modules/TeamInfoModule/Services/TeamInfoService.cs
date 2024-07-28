using EvoSC.Common.Interfaces;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.TeamInfoModule.Config;
using EvoSC.Modules.Official.TeamInfoModule.Interfaces;
using EvoSC.Modules.Official.TeamInfoModule.Models;
using LinqToDB.Common;

namespace EvoSC.Modules.Official.TeamInfoModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class TeamInfoService(
    IServerClient server,
    IManialinkManager manialinks,
    ITeamInfoSettings settings
) : ITeamInfoService
{
    private const string WidgetTemplate = "TeamInfoModule.TeamInfoWidget";

    private bool _modeIsTeams;
    private int _currentRound;
    private int _team1Points;
    private int _team2Points;

    public async Task InitializeModuleAsync()
    {
        await server.Remote.TriggerModeScriptEventArrayAsync("Trackmania.GetScores");
    }

    public async Task<dynamic> GetWidgetDataAsync()
    {
        var modeScriptSettings = await GetModeScriptTeamSettingsAsync();
        var infoBoxText = await GetInfoBoxTextAsync(modeScriptSettings);
        var team1 = await server.Remote.GetTeamInfoAsync(1);
        var team2 = await server.Remote.GetTeamInfoAsync(2);
        var team1MatchPoint = await DoesTeamHaveMatchPointAsync(_team1Points, _team2Points, modeScriptSettings.PointsLimit,
            modeScriptSettings.PointsGap);
        var team2MatchPoint = await DoesTeamHaveMatchPointAsync(_team2Points, _team1Points, modeScriptSettings.PointsLimit,
            modeScriptSettings.PointsGap);

        return new
        {
            team1,
            team2,
            infoBoxText,
            team1MatchPoint,
            team2MatchPoint,
            settings,
            roundNumber = _currentRound,
            team1Points = _team1Points,
            team2Points = _team2Points,
            neutralEmblemUrl = modeScriptSettings.NeutralEmblemUrl
        };
    }

    public async Task<ModeScriptTeamSettings> GetModeScriptTeamSettingsAsync()
    {
        var modeScriptSettings = await server.Remote.GetModeScriptSettingsAsync();
        var defaultSettings = new ModeScriptTeamSettings();

        return new ModeScriptTeamSettings
        {
            PointsLimit =
                modeScriptSettings.TryGetValue("S_PointsLimit", out var pointsLimit)
                    ? (int)pointsLimit
                    : defaultSettings.PointsLimit,
            FinishTimeout =
                modeScriptSettings.TryGetValue("S_FinishTimeout", out var finishTimeout)
                    ? (int)finishTimeout
                    : defaultSettings.FinishTimeout,
            MaxPointsPerRound =
                modeScriptSettings.TryGetValue("S_MaxPointsPerRound", out var maxPointsPerRound)
                    ? (int)maxPointsPerRound
                    : defaultSettings.MaxPointsPerRound,
            PointsGap =
                modeScriptSettings.TryGetValue("S_PointsGap", out var pointsGap)
                    ? (int)pointsGap
                    : defaultSettings.PointsGap,
            UseCustomPointsRepartition =
                modeScriptSettings.TryGetValue("S_UseCustomPointsRepartition", out var useCustomPointsRepartition)
                    ? (bool)useCustomPointsRepartition
                    : defaultSettings.UseCustomPointsRepartition,
            CumulatePoints =
                modeScriptSettings.TryGetValue("S_CumulatePoints", out var cumulatePoints)
                    ? (bool)cumulatePoints
                    : defaultSettings.CumulatePoints,
            RoundsPerMap =
                modeScriptSettings.TryGetValue("S_RoundsPerMap", out var roundsPerMap)
                    ? (int)roundsPerMap
                    : defaultSettings.RoundsPerMap,
            MapsPerMatch =
                modeScriptSettings.TryGetValue("S_MapsPerMatch", out var mapsPerMatch)
                    ? (int)mapsPerMatch
                    : defaultSettings.MapsPerMatch,
            UseTieBreak =
                modeScriptSettings.TryGetValue("S_UseTieBreak", out var useTieBreak)
                    ? (bool)useTieBreak
                    : defaultSettings.UseTieBreak,
            WarmUpNb =
                modeScriptSettings.TryGetValue("S_WarmUpNb", out var warmupNb)
                    ? (int)warmupNb
                    : defaultSettings.WarmUpNb,
            WarmUpDuration =
                modeScriptSettings.TryGetValue("S_WarmUpDuration", out var warmupDuration)
                    ? (int)warmupDuration
                    : defaultSettings.WarmUpDuration,
            UseAlternateRules = modeScriptSettings.TryGetValue("S_UseAlternateRules", out var useAlternateRules)
                ? (bool)useAlternateRules
                : defaultSettings.UseAlternateRules,
            NeutralEmblemUrl = modeScriptSettings.TryGetValue("S_NeutralEmblemUrl", out var neutralEmblemUrl)
                ? (string)neutralEmblemUrl
                : defaultSettings.NeutralEmblemUrl,
        };
    }

    public Task<string?> GetInfoBoxTextAsync(ModeScriptTeamSettings modeScriptTeamSettings)
    {
        var infoBoxText = new List<string>();

        //Add point limit and gap
        if (modeScriptTeamSettings.PointsLimit > 0)
        {
            infoBoxText.Add("FIRST TO " + modeScriptTeamSettings.PointsLimit);

            if (modeScriptTeamSettings.PointsGap > 1)
            {
                infoBoxText.Add($"GAP {modeScriptTeamSettings.PointsGap}");
            }
        }

        //Add rounds per map
        if (modeScriptTeamSettings.RoundsPerMap > 0)
        {
            infoBoxText.Add($"{modeScriptTeamSettings.RoundsPerMap} ROUNDS/MAP");
        }

        return Task.FromResult(infoBoxText.IsNullOrEmpty() ? null : string.Join(" | ", infoBoxText));
    }

    public Task<bool> DoesTeamHaveMatchPointAsync(int teamPoints, int opponentPoints, int pointsLimit, int pointsGap)
    {
        return Task.FromResult(teamPoints >= pointsLimit - 1 &&
                               teamPoints - (pointsGap - 1) >= opponentPoints);
    }

    public async Task SendTeamInfoWidgetEveryoneAsync()
    {
        await manialinks.SendPersistentManialinkAsync(WidgetTemplate, await GetWidgetDataAsync());
    }

    public async Task HideTeamInfoWidgetEveryoneAsync()
    {
        await manialinks.HideManialinkAsync(WidgetTemplate);
    }

    public async Task UpdateRoundNumberAsync(int round)
    {
        _currentRound = round;
        await SendTeamInfoWidgetEveryoneAsync();
    }

    public async Task UpdatePointsAsync(int team1Points, int team2Points)
    {
        _team1Points = team1Points;
        _team2Points = team2Points;
        await SendTeamInfoWidgetEveryoneAsync();
    }

    public Task<bool> GetModeIsTeamsAsync()
    {
        return Task.FromResult(_modeIsTeams);
    }

    public Task SetModeIsTeamsAsync(bool modeIsTeams)
    {
        _modeIsTeams = modeIsTeams;
        
        return Task.CompletedTask;
    }
}
