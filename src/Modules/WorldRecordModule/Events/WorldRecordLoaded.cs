using EvoSC.Modules.Official.WorldRecordModule.Models;

namespace EvoSC.Modules.Official.WorldRecordModule.Events;

public class WorldRecordLoaded: EventArgs
{
    public required WorldRecord Record { get; init; }
}
