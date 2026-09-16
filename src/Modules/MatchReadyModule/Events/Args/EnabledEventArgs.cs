using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.MatchReadyModule.Events.Args;

public class EnabledEventArgs : EventArgs
{
    public required IEnumerable<IPlayer> Players { get; init; }
}
