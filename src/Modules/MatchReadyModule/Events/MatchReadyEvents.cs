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
    PlayerReadyChanged
}
