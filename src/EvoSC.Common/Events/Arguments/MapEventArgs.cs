using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Common.Events.Arguments;

public class MapEventArgs : EventArgs
{
    public IMap Map { get; init; }
}
