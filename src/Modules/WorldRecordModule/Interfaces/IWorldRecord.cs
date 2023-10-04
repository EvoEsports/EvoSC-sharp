using EvoSC.Common.Interfaces.Util;
using EvoSC.Modules.Official.WorldRecordModule.Models;

namespace EvoSC.Modules.Official.WorldRecordModule.Interfaces;

public interface IWorldRecord
{
    public string PlayerName { get; init; }
    public IRaceTime Time { get; init; }
    public WorldRecordSource Source { get; init; }
}
