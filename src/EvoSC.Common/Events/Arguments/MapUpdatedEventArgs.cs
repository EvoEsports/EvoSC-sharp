using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Common.Events.Arguments;

public class MapUpdatedEventArgs : MapEventArgs
{
    /// <summary>
    /// Information about the previous map that was updated.
    /// </summary>
    public IMap OldMap { get; set; }
}
