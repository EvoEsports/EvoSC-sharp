using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.PlayerRecords.Events;

namespace EvoSC.Modules.Official.PlayerRecords.Interfaces;

/// <summary>
/// Exported so other modules can read and write personal records without a reference to this
/// module. The record types themselves live in the shared framework, so both sides exchange the
/// same data.
/// </summary>
[Export]
public interface IPlayerRecordsService
{
    /// <summary>
    /// Set a new PB record for a player.
    /// </summary>
    /// <param name="player">The player to set the record for.</param>
    /// <param name="map">The map to set the record in.</param>
    /// <param name="score">The score of this record.</param>
    /// <param name="checkpoints">Checkpoint times that was driven for this record.</param>
    /// <returns></returns>
    public Task<(IPlayerRecord, RecordUpdateStatus)> SetPbRecordAsync(IPlayer player, IMap map, int score, IEnumerable<int> checkpoints);

    /// <summary>
    /// Get a record of a player.
    /// </summary>
    /// <param name="player">The player to get the record of.</param>
    /// <param name="map">The map to get the record from.</param>
    /// <returns></returns>
    public Task<IPlayerRecord?> GetPlayerRecordAsync(IPlayer player, IMap map);
}
