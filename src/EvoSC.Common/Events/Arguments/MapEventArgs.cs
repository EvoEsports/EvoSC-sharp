using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Common.Events.Arguments;

public class MapEventArgs : EventArgs
{
    /// <summary>
    /// The map that was changed.
    /// </summary>
    public IMap Map { get; init; }
}
