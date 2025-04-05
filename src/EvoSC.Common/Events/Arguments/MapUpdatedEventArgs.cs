using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Common.Events.Arguments;

public class MapUpdatedEventArgs : MapEventArgs
{
    public IMap OldMap { get; set; }
}
