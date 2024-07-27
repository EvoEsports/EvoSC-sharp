using EvoSC.Modules.Official.TeamInfoModule.Models;

namespace EvoSC.Modules.Official.TeamInfoModule.Interfaces;

public interface ITeamInfoService
{
    /// <summary>
    /// Triggers detection if team mode is active.
    /// </summary>
    /// <returns></returns>
    public Task InitializeModuleAsync();

    /// <summary>
    /// Gets the current mode script settings for teams mode.
    /// </summary>
    /// <returns></returns>
    public Task<ModeScriptTeamSettings> GetModeScriptTeamSettings();

    /// <summary>
    /// Get the text displayed at the bottom of the team info widget.
    /// </summary>
    /// <param name="modeScriptTeamSettings"></param>
    /// <returns></returns>
    public Task<string?> GetInfoBoxText(ModeScriptTeamSettings modeScriptTeamSettings);

    /// <summary>
    /// Gets all necessary data for the widget.
    /// </summary>
    /// <returns></returns>
    public Task<dynamic> GetManialinkDataAsync();

    /// <summary>
    /// Determines whether a team has match point.
    /// </summary>
    /// <param name="teamPoints"></param>
    /// <param name="opponentPoints"></param>
    /// <param name="pointsLimit"></param>
    /// <param name="pointsGap"></param>
    /// <returns></returns>
    public Task<bool> DoesTeamHaveMatchPoint(int teamPoints, int opponentPoints, int pointsLimit, int pointsGap);

    /// <summary>
    /// Sends the team info widget to the given player.
    /// </summary>
    /// <param name="playerLogin"></param>
    /// <returns></returns>
    public Task SendTeamInfoWidgetAsync(string playerLogin);

    /// <summary>
    /// Sends the team info widget to all players.
    /// </summary>
    /// <returns></returns>
    public Task SendTeamInfoWidgetEveryoneAsync();

    /// <summary>
    /// Hides the team info widget for all players.
    /// </summary>
    /// <returns></returns>
    public Task HideTeamInfoWidgetEveryoneAsync();

    /// <summary>
    /// Sets the current round number.
    /// </summary>
    /// <param name="round"></param>
    /// <returns></returns>
    public Task UpdateRoundNumberAsync(int round);

    /// <summary>
    /// Updates the points for both teams.
    /// </summary>
    /// <param name="team1Points"></param>
    /// <param name="team2Points"></param>
    /// <returns></returns>
    public Task UpdatePointsAsync(int team1Points, int team2Points);
    
    /// <summary>
    /// Gets whether the widget should be displayed to players.
    /// </summary>
    /// <returns></returns>
    public Task<bool> GetWidgetVisibilityAsync();
    
    /// <summary>
    /// Sets whether the widget should be displayed to players.
    /// </summary>
    /// <returns></returns>
    public Task SetWidgetVisibilityAsync(bool visible);
    
    /// <summary>
    /// Tells whether the service is currently active for teams mode.
    /// </summary>
    /// <returns></returns>
    public Task<bool> GetModeIsTeams();
    
    /// <summary>
    /// Sets whether the service is active for teams mode or not.
    /// </summary>
    /// <param name="modeIsTeams"></param>
    /// <returns></returns>
    public Task SetModeIsTeams(bool modeIsTeams);
}
