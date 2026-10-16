using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.SetNameModule.Events;

/// <summary>
/// Exported so other modules can handle the nickname event without a reference to this module:
/// the event manager invokes handlers by reflection, so the handler parameter has to be the
/// very same type on both sides.
/// </summary>
[Export]
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
