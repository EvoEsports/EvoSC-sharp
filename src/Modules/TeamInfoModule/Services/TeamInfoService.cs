using System.Text;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.TeamInfoModule.Interfaces;

namespace EvoSC.Modules.Official.TeamInfoModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class TeamInfoService(IServerClient server, IManialinkManager manialinks) : ITeamInfoService
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
        var mapPoint = 0;

        if (await DoesTeamHaveMatchPoint(_team1Points, _team2Points, modeScriptSettings.PointsLimit,
                modeScriptSettings.PointsGap))
        {
            mapPoint = 1;
        }

        if (await DoesTeamHaveMatchPoint(_team2Points, _team1Points, modeScriptSettings.PointsLimit,
                modeScriptSettings.PointsGap))
        {
            mapPoint = 2;
        }

        return new
        {
            team1,
            team2,
            infoBoxText,
            mapPoint,
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
            PointsGap = (int)modeScriptSettings["S_PointsGap"],
            RoundsPerMap = (int)modeScriptSettings["S_RoundsPerMap"]
        };
    }

    public Task<string?> GetInfoBoxText(ModeScriptTeamSettings modeScriptTeamSettings)
    {
        var output = new StringBuilder();

        if (modeScriptTeamSettings.PointsLimit > 0)
        {
            output.Append("FIRST TO " + modeScriptTeamSettings.PointsLimit);

            if (modeScriptTeamSettings.IsTennisMode())
            {
                output.Append(' ').Append($"(TENNIS {modeScriptTeamSettings.PointsGap})");
            }
        }
        else if (output.Length == 0)
        {
            return Task.FromResult<string?>(null);
        }

        return Task.FromResult<string?>(output.ToString().ToUpper());
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
