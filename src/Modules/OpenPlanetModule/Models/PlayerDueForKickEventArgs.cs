using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.OpenPlanetModule.Models;

public class PlayerDueForKickEventArgs : EventArgs
{
    public required IPlayer Player { get; init; }
}
