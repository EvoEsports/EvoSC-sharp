using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.SetName.Events;

public class NicknameUpdatedEventArgs : EventArgs
{
    /// <summary>
    /// The player that changed their name.
    /// </summary>
    public required IPlayer Player { get; init; }
    
    /// <summary>
    /// The previous name of the player.
    /// </summary>
    public required string OldName { get; init; }
    
    /// <summary>
    /// The new name of the player.
    /// </summary>
    public required string NewName { get; init; }
}
