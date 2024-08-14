using EvoSC.Common.Interfaces;
using EvoSC.Common.Models;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Common.Util.MatchSettings.Models.ModeScriptSettingsModels;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.TeamInfoModule.Config;
using EvoSC.Modules.Official.TeamInfoModule.Interfaces;
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
    private bool _executeManiaScript;

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
        var team1MatchPoint = await DoesTeamHaveMatchPointAsync(_team1Points, _team2Points,
            modeScriptSettings.PointsLimit,
            modeScriptSettings.PointsGap);
        var team2MatchPoint = await DoesTeamHaveMatchPointAsync(_team2Points, _team1Points,
            modeScriptSettings.PointsLimit,
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
            executeManiaScript = _executeManiaScript,
            neutralEmblemUrl = modeScriptSettings.NeutralEmblemUrl
        };
    }

    public async Task<TeamsModeScriptSettings> GetModeScriptTeamSettingsAsync()
    {
        var modeScriptSettings = await server.Remote.GetModeScriptSettingsAsync();

        return new TeamsModeScriptSettings
        {
            PointsLimit = (int)modeScriptSettings.GetValueOrDefault("S_PointsLimit", 5),
            FinishTimeout = (int)modeScriptSettings.GetValueOrDefault("S_FinishTimeout", -1),
            MaxPointsPerRound = (int)modeScriptSettings.GetValueOrDefault("S_MaxPointsPerRound", 6),
            PointsGap = (int)modeScriptSettings.GetValueOrDefault("S_PointsGap", 1),
            RoundsPerMap = (int)modeScriptSettings.GetValueOrDefault("S_RoundsPerMap", -1),
            MapsPerMatch = (int)modeScriptSettings.GetValueOrDefault("S_MapsPerMatch", -1),
            NeutralEmblemUrl = (string)modeScriptSettings.GetValueOrDefault("S_NeutralEmblemUrl", "")
        };
    }

    public Task<string?> GetInfoBoxTextAsync(TeamsModeScriptSettings modeScriptTeamSettings)
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

    public Task<bool> DoesTeamHaveMatchPointAsync(int teamPoints, int opponentPoints, int? pointsLimit, int? pointsGap)
    {
        if (pointsLimit == null)
        {
            return Task.FromResult(false);
        }

        pointsGap ??= 1;

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

    public async Task UpdatePointsAsync(int team1Points, int team2Points, bool executeManiaScript)
    {
        _team1Points = team1Points;
        _team2Points = team2Points;
        _executeManiaScript = executeManiaScript;
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

    public bool ShouldUpdateTeamPoints(ModeScriptSection section)
    {
        return section is ModeScriptSection.EndRound or ModeScriptSection.PreEndRound or ModeScriptSection.Undefined;
    }

    public bool ShouldExecuteManiaScript(ModeScriptSection section)
    {
        return section is ModeScriptSection.PreEndRound;
    }
}
