using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.PlayerRecords.Events;

/// <summary>
/// Exported so other modules can handle the personal best event without a reference to this
/// module: the event manager invokes handlers by reflection, so the handler parameter has to be
/// the very same type on both sides.
/// </summary>
[Export]
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
