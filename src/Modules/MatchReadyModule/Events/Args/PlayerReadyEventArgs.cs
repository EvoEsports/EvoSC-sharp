using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.MatchReadyModule.Events.Args;

public class PlayerReadyEventArgs : EventArgs
{
    public required IPlayer Player { get; init; }
    public required bool IsReady { get; init; }
}
