using EvoSC.Modules.Official.WorldRecordModule.Models;

namespace EvoSC.Modules.Official.WorldRecordModule.Events;

public class WorldRecordLoadedEventArgs: EventArgs
{
    /// <summary>
    /// The world record that was loaded.
    /// </summary>
    public required WorldRecord Record { get; init; }
}
