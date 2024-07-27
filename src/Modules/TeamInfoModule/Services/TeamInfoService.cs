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

    private bool _modeIsTeams = true;
    private bool _widgetShouldBeDisplayed;
    private int _currentRound;
    private int _team1Points;
    private int _team2Points;

    public async Task InitializeModuleAsync()
    {
        //TODO: check if teams mode is active -> Maniaplanet.Mode.GetUseTeams?

        // var getUseTeamsResponse = await server.Remote.TriggerModeScriptEventArrayAsync("Maniaplanet.Mode.GetUseTeams");

        //TODO: get current round number

        if (_modeIsTeams)
        {
            await server.Remote.TriggerModeScriptEventArrayAsync("Trackmania.GetScores");
        }
        else
        {
            await HideTeamInfoWidgetEveryoneAsync();
        }
    }

    public async Task<dynamic> GetManialinkDataAsync()
    {
        var team1 = await server.Remote.GetTeamInfoAsync(1);
        var team2 = await server.Remote.GetTeamInfoAsync(2);
        var modeScriptSettings = await GetTeamModeSettingsAsync();
        var infoBoxText = await GetInfoBoxText(modeScriptSettings);
        var team1MatchPoint = false;
        var team2MatchPoint = false;

        if (await DoesTeamHaveMatchPoint(_team1Points, _team2Points, modeScriptSettings.PointsLimit,
                modeScriptSettings.PointsGap))
        {
            team1MatchPoint = true;
        }

        if (await DoesTeamHaveMatchPoint(_team2Points, _team1Points, modeScriptSettings.PointsLimit,
                modeScriptSettings.PointsGap))
        {
            team2MatchPoint = true;
        }

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
            team2Points = _team2Points
        };
    }

    public async Task<ModeScriptTeamSettings> GetTeamModeSettingsAsync()
    {
        var modeScriptSettings = await server.Remote.GetModeScriptSettingsAsync();

        return new ModeScriptTeamSettings
        {
            PointsLimit = (int)modeScriptSettings["S_PointsLimit"],
            FinishTimeout = (int)modeScriptSettings["S_FinishTimeout"],
            MaxPointsPerRound = (int)modeScriptSettings["S_MaxPointsPerRound"],
            PointsGap = (int)modeScriptSettings["S_PointsGap"],
            UseCustomPointsRepartition = (bool)modeScriptSettings["S_UseCustomPointsRepartition"],
            CumulatePoints = (bool)modeScriptSettings["S_CumulatePoints"],
            RoundsPerMap = (int)modeScriptSettings["S_RoundsPerMap"],
            MapsPerMatch = (int)modeScriptSettings["S_MapsPerMatch"],
            UseTieBreak = (bool)modeScriptSettings["S_UseTieBreak"],
            WarmUpNb = (int)modeScriptSettings["S_WarmUpNb"],
            WarmUpDuration = (int)modeScriptSettings["S_WarmUpDuration"],
            UseAlternateRules = (bool)modeScriptSettings["S_UseAlternateRules"]
        };
    }

    public Task<string?> GetInfoBoxText(ModeScriptTeamSettings modeScriptTeamSettings)
    {
        var infoBoxText = new List<string>();

        if (modeScriptTeamSettings.PointsLimit > 0)
        {
            infoBoxText.Add("FIRST TO " + modeScriptTeamSettings.PointsLimit);

            if (modeScriptTeamSettings.PointsGap > 1)
            {
                infoBoxText.Add($"(TENNIS {modeScriptTeamSettings.PointsGap})");
            }
        }
        else if (modeScriptTeamSettings.RoundsPerMap > 0)
        {
            infoBoxText.Add($"{modeScriptTeamSettings.RoundsPerMap} ROUNDS PER MAP");
        }
        else if (infoBoxText.IsNullOrEmpty())
        {
            return Task.FromResult<string?>(null);
        }

        return Task.FromResult<string?>(string.Join(' ', infoBoxText));
    }

    public Task<bool> DoesTeamHaveMatchPoint(int teamPoints, int opponentPoints, int pointsLimit, int pointsGap)
    {
        return Task.FromResult(teamPoints >= pointsLimit - 1 &&
                               teamPoints - (pointsGap - 1) >= opponentPoints);
    }

    public async Task SendTeamInfoWidgetAsync(string playerLogin)
    {
        if (!_widgetShouldBeDisplayed)
        {
            return;
        }

        await manialinks.SendManialinkAsync(playerLogin, WidgetTemplate, await GetManialinkDataAsync());
    }

    public async Task SendTeamInfoWidgetEveryoneAsync()
    {
        _widgetShouldBeDisplayed = true;
        await manialinks.SendManialinkAsync(WidgetTemplate, await GetManialinkDataAsync());
    }

    public async Task HideTeamInfoWidgetEveryoneAsync()
    {
        _widgetShouldBeDisplayed = false;
        await manialinks.HideManialinkAsync(WidgetTemplate);
    }

    public async Task UpdateRoundNumberAsync(int round)
    {
        _currentRound = round;
        _widgetShouldBeDisplayed = true;
        await SendTeamInfoWidgetEveryoneAsync();
    }

    public async Task UpdatePointsAsync(int team1Points, int team2Points)
    {
        _team1Points = team1Points;
        _team2Points = team2Points;
        await SendTeamInfoWidgetEveryoneAsync();
    }
}
