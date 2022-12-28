using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.PlayerRecords.Interfaces.Models;

namespace EvoSC.Modules.Official.PlayerRecords;

public class PbRecordUpdateEventArgs : EventArgs
{
    public required IPlayer Player { get; init; }
    public required IMap Map { get; init; }
    public required IPlayerRecord Record { get; init; }
    public required RecordUpdateStatus Status { get; init; }
}
