using EvoSC.Common.Interfaces.Util;
using EvoSC.Modules.Official.WorldRecordModule.Interfaces;

namespace EvoSC.Modules.Official.WorldRecordModule.Models;

public class WorldRecord : IWorldRecord
{
    public required string PlayerName { get; init; }
    public required IRaceTime Time { get; init; }
    public required WorldRecordSource Source { get; init; }
}
