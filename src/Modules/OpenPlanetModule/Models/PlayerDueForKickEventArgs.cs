using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.OpenPlanetModule.Models;

public class PlayerDueForKickEventArgs : EventArgs
{
    /// <summary>
    /// The player that is due to be kicked.
    /// </summary>
    public required IPlayer Player { get; init; }
}
