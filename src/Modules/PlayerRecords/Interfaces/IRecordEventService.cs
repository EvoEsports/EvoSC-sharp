using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.PlayerRecords.Interfaces.Models;

namespace EvoSC.Modules.Official.PlayerRecords.Interfaces;

public interface IRecordEventService
{
    /// <summary>
    /// Check the waypoint and add/update a record if it's new or better.
    /// </summary>
    /// <param name="waypoint">Information about the waypoint to check.</param>
    /// <returns></returns>
    public Task CheckWaypointAsync(WayPointEventArgs waypoint);
    
    /// <summary>
    /// Sends an update to the in-game chat about a new record.
    /// </summary>
    /// <param name="player">The player that got the record.</param>
    /// <param name="record">Information about the new record.</param>
    /// <returns></returns>
    public Task SendRecordUpdateToChatAsync(IPlayerRecord record);
}
