using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.SpectatorTargetInfoModule.Models;
using GbxRemoteNet.Structs;

namespace EvoSC.Modules.Official.SpectatorTargetInfoModule.Interfaces;

public interface ISpectatorTargetInfoService
{
    /// <summary>
    /// Initialize the module on controller start.
    /// </summary>
    /// <returns></returns>
    public Task InitializeAsync();

    /// <summary>
    /// Register a new checkpoint time for the ongoing round.
    /// </summary>
    /// <param name="playerLogin"></param>
    /// <param name="checkpointIndex"></param>
    /// <param name="checkpointTime"></param>
    /// <returns></returns>
    public Task AddCheckpointAsync(string playerLogin, int checkpointIndex, int checkpointTime);

    /// <summary>
    /// Clears all registered checkpoint times.
    /// </summary>
    /// <returns></returns>
    public Task ClearCheckpointsAsync();
    
    /// <summary>
    /// Retrieve an IOnlinePlayer instance by their login.
    /// </summary>
    /// <param name="playerLogin"></param>
    /// <returns></returns>
    public Task<IOnlinePlayer> GetOnlinePlayerByLoginAsync(string playerLogin);

    /// <summary>
    /// Get the login of a player by their dedicated server ID.
    /// </summary>
    /// <param name="targetPlayerIdDedicated"></param>
    /// <returns></returns>
    public Task<string?> GetLoginOfDedicatedPlayerAsync(int targetPlayerIdDedicated);

    /// <summary>
    /// Set the spectator target for a player.
    /// </summary>
    /// <param name="spectatorLogin"></param>
    /// <param name="targetLogin"></param>
    /// <returns></returns>
    public Task<IOnlinePlayer?> SetSpectatorTargetAsync(string spectatorLogin, string targetLogin);
    
    /// <summary>
    /// Set the spectator target for a player and display the widget to them.
    /// </summary>
    /// <param name="spectatorLogin"></param>
    /// <param name="targetLogin"></param>
    /// <returns></returns>
    public Task SetSpectatorTargetAndSendAsync(string spectatorLogin, string targetLogin);

    /// <summary>
    /// Remove a player from the spectators list.
    /// </summary>
    /// <param name="spectatorLogin"></param>
    /// <returns></returns>
    public Task RemovePlayerFromSpectatorsListAsync(string spectatorLogin);

    /// <summary>
    /// Gets the logins of a players spectating the given target.
    /// </summary>
    /// <param name="targetPlayer"></param>
    /// <returns></returns>
    public IEnumerable<string> GetLoginsOfPlayersSpectatingTarget(IOnlinePlayer targetPlayer);

    /// <summary>
    /// Calculates the time difference between two CheckpointData in milliseconds.
    /// </summary>
    /// <param name="leadingCheckpointData"></param>
    /// <param name="targetCheckpointData"></param>
    /// <returns></returns>
    public int GetTimeDifference(CheckpointData leadingCheckpointData, CheckpointData targetCheckpointData);

    /// <summary>
    /// Calculates the time difference between two given times in milliseconds.
    /// </summary>
    /// <param name="leadingCheckpointTime"></param>
    /// <param name="targetCheckpointTime"></param>
    /// <returns></returns>
    public int GetTimeDifference(int leadingCheckpointTime, int targetCheckpointTime);

    /// <summary>
    /// Gets the hex team color for the given team.
    /// </summary>
    /// <param name="team"></param>
    /// <returns></returns>
    public string GetTeamColorAsync(PlayerTeam team);

    /// <summary>
    /// Gets the highest checkpoint ID the given player has reached in the ongoing round.
    /// </summary>
    /// <param name="playerLogin"></param>
    /// <returns></returns>
    public int GetLastCheckpointIndexOfPlayer(IOnlinePlayer playerLogin);
    
    /// <summary>
    /// Returns the CheckpointGroups for all collected checkpoints.
    /// </summary>
    /// <returns></returns>
    public Dictionary<int, CheckpointsGroup> GetCheckpointTimes();

    /// <summary>
    /// Resets the widget for all spectating players.
    /// </summary>
    /// <returns></returns>
    public Task ResetWidgetForSpectatorsAsync();

    /// <summary>
    /// Send the widget to the given player.
    /// </summary>
    /// <param name="spectatorLogin"></param>
    /// <param name="targetPlayer"></param>
    /// <returns></returns>
    public Task SendSpectatorInfoWidgetAsync(string spectatorLogin, IOnlinePlayer targetPlayer);

    /// <summary>
    /// Send the widget to the given players.
    /// </summary>
    /// <param name="spectatorLogin"></param>
    /// <param name="targetPlayer"></param>
    /// <returns></returns>
    public Task SendSpectatorInfoWidgetAsync(IEnumerable<string> spectatorLogins, IOnlinePlayer targetPlayer, int targetPlayerRank, int timeDifference);

    /// <summary>
    /// Send the widget to the given player.
    /// </summary>
    /// <param name="spectatorLogin"></param>
    /// <param name="targetPlayer"></param>
    /// <returns></returns>
    public Task SendSpectatorInfoWidgetAsync(string spectatorLogin, IOnlinePlayer targetPlayer, int targetPlayerRank, int timeDifference);

    /// <summary>
    /// Hides the widget for all players.
    /// </summary>
    /// <returns></returns>
    public Task HideSpectatorInfoWidgetAsync();

    /// <summary>
    /// Hides the widget for the given player.
    /// </summary>
    /// <param name="playerLogin"></param>
    /// <returns></returns>
    public Task HideSpectatorInfoWidgetAsync(string playerLogin);

    /// <summary>
    /// Sends the script responsible for reporting back the current spectator target from the player to the module.
    /// </summary>
    /// <returns></returns>
    public Task SendReportSpectatorTargetManialinkAsync();

    /// <summary>
    /// Retrieve and cache the latest team infos.
    /// </summary>
    /// <returns></returns>
    public Task UpdateTeamInfoAsync();
    
    /// <summary>
    /// Updates whether team mode is active or not.
    /// </summary>
    /// <returns></returns>
    public Task UpdateIsTeamsModeAsync();

    /// <summary>
    /// Hides the default game mode UI.
    /// </summary>
    /// <returns></returns>
    public Task HideGameModeUiAsync();
}
