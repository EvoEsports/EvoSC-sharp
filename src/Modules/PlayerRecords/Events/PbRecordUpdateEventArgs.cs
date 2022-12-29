using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.PlayerRecords.Interfaces.Models;

namespace EvoSC.Modules.Official.PlayerRecords.Events;

public class PbRecordUpdateEventArgs : EventArgs
{
    /// <summary>
    /// The player that has this pb.
    /// </summary>
    public required IPlayer Player { get; init; }
    
    /// <summary>
    /// The map which the time was driven on.
    /// </summary>
    public required IMap Map { get; init; }
    
    /// <summary>
    /// Information about the record.
    /// </summary>
    public required IPlayerRecord Record { get; init; }
    
    /// <summary>
    /// The status of this record, whether it is new, updated etc.
    /// </summary>
    public required RecordUpdateStatus Status { get; init; }
}
