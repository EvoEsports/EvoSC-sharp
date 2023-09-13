using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.MatchReadyModule.Events.Args;

public class AllPlayersReadyEventArgs : EventArgs
{
    public required IEnumerable<IPlayer> ReadyPlayers { get; init; }
}
