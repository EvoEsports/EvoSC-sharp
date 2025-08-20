namespace EvoSC.Modules.Official.MatchReadyModule.Events;

public enum MatchReadyEvents
{
    /// <summary>
    /// Raised when the required amount of players to be ready are ready.
    /// </summary>
    AllPlayersReady,
    
    /// <summary>
    /// Raised when the ready status of a player has been changed.
    /// </summary>
    PlayerReadyChanged,
    
    /// <summary>
    /// Raised when the ready widget and service enabled.
    /// </summary>
    Enabled,
    
    /// <summary>
    /// Raised when the ready widget and service enabled.
    /// </summary>
    Disabled
}
