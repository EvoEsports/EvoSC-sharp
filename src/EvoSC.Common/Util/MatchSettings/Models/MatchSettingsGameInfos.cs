namespace EvoSC.Common.Util.MatchSettings.Models;

/// <summary>
/// Represents the "gameinfo" section of a match settings file.
/// </summary>
public class MatchSettingsGameInfos
{
    /// <summary>
    /// Game mode, not really used and is typically always 0.
    /// </summary>
    public int GameMode { get; set; }
    
    /// <summary>
    /// The amount of time to wait between rounds/map change.
    /// </summary>
    public int ChatTime { get; set; }
    
    /// <summary>
    /// The amount of time for other players to finish after the first player has finished.
    /// </summary>
    public int FinishTimeout { get; set; }
    
    /// <summary>
    /// Warm up duration.
    /// </summary>
    public bool AllWarmupDuration { get; set; }
    
    /// <summary>
    /// Whether to disable respawn or not. Not used, use the script setting 'RespawnBehavior' instead.
    /// See: https://wiki.trackmania.io/en/dedicated-server/Usage/OfficialGameModesSettings#s_respawnbehaviour
    /// </summary>
    public bool DisableRespawn { get; set; }
    
    /// <summary>
    /// Disallow players to hide opponents.
    /// </summary>
    public bool ForceShowAllOpponents { get; set; }
    
    /// <summary>
    /// The name of the current game mode.
    /// </summary>
    public required string ScriptName { get; set; }
}
