using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Common.Events.Arguments;

public class NewPlayerAddedEventArgs : EventArgs
{
    public required IPlayer Player { get; init; }
}
