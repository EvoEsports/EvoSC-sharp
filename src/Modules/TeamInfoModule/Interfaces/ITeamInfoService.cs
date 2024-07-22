﻿using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.TeamInfoModule.Interfaces;

public interface ITeamInfoService
{
    public Task InitializeModuleAsync();
    
    public Task<dynamic> GetManialinkDataAsync();

    public Task<ModeScriptTeamSettings> GetTeamModeSettingsAsync();

    public Task<string?> GetInfoBoxText(ModeScriptTeamSettings modeScriptTeamSettings);

    public Task<bool> DoesTeamHaveMatchPoint(int teamPoints, int opponentPoints, int pointsLimit, int pointsGap);
    
    public Task SendTeamInfoWidgetAsync(string playerLogin);

    public Task SendTeamInfoWidgetEveryoneAsync();

    public Task HideTeamInfoWidgetEveryoneAsync();

    public Task UpdateRoundNumber(int round);

    public Task RequestScoresFromServerAsync();
    
    public Task UpdatePointsAsync(int team1Points, int team2Points);
}
