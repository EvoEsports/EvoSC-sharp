using System.Text;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.TeamInfoModule.Interfaces;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.TeamInfoModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class TeamInfoService(IServerClient server, IManialinkManager manialinks, ILogger<TeamInfoService> logger)
    : ITeamInfoService
{
    private const string WidgetTemplate = "TeamInfoModule.TeamInfoWidget";

    private bool _modeIsTeams = true;
    private bool _widgetShouldBeDisplayed;
    private int _currentRound;
    private int _team1Points;
    private int _team2Points;

    public async Task InitializeModuleAsync()
    {
        //TODO: check if teams mode is active -> Maniaplanet.Mode.GetUseTeams

        // var getUseTeamsResponse = await server.Remote.CallMethodAsync("Maniaplanet.Mode.GetUseTeams");
        // logger.LogInformation("response: {response}", getUseTeamsResponse.ResponseData.ToString());

        logger.LogInformation("Initializing...");

        if (_modeIsTeams)
        {
            _widgetShouldBeDisplayed = true;
            await RequestScoresFromServerAsync();
            // await SendTeamInfoWidgetEveryoneAsync();

            logger.LogInformation("Mode is teams.");
        }
        else
        {
            _widgetShouldBeDisplayed = false;
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

        if (await DoesTeamHaveMapPoint(_team1Points, _team2Points, modeScriptSettings.PointsLimit,
                modeScriptSettings.PointsGap))
        {
            mapPoint = 1;
        }

        if (await DoesTeamHaveMapPoint(_team2Points, _team1Points, modeScriptSettings.PointsLimit,
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
            team2Points = _team2Points,
            team1GainedPoints = 0,
            team2GainedPoints = 0
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
            return null;
        }

        return Task.FromResult(output.ToString().ToUpper());
    }

    public Task<bool> DoesTeamHaveMapPoint(int teamPoints, int opponentPoints, int pointsLimit, int pointsGap)
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
        if (!_widgetShouldBeDisplayed)
        {
            return;
        }

        await manialinks.SendManialinkAsync(WidgetTemplate, await GetManialinkDataAsync());
    }

    public async Task HideTeamInfoWidgetAsync(string playerLogin)
    {
        await manialinks.HideManialinkAsync(playerLogin, WidgetTemplate);
    }

    public async Task HideTeamInfoWidgetEveryoneAsync()
    {
        await manialinks.HideManialinkAsync(WidgetTemplate);
    }

    public Task UpdateRoundNumber(int round)
    {
        _currentRound = round;

        return Task.CompletedTask;
    }

    public async Task RequestScoresFromServerAsync()
    {
        await server.Remote.TriggerModeScriptEventArrayAsync("Trackmania.GetScores");
    }

    public async Task UpdatePointsAsync(int team1Points, int team2Points)
    {
        _team1Points = team1Points;
        _team2Points = team2Points;

        await SendTeamInfoWidgetEveryoneAsync();
    }

    public async Task ClearPoints()
    {
        _team1Points = 0;
        _team2Points = 0;

        await SendTeamInfoWidgetEveryoneAsync();
    }

    public Task SetWidgetVisibility(bool visible)
    {
        _widgetShouldBeDisplayed = visible;

        return Task.CompletedTask;
    }
}
