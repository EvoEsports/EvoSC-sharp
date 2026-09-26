using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Database.Models.PlayerRecords;

namespace EvoSC.Modules.Official.PlayerRecords.Interfaces;

/// <summary>
/// Exported so other modules can read the records table without a reference to this module. The
/// record entity is part of the shared framework, which is what lets a module's own table hold a
/// foreign key to it.
/// </summary>
[Export]
public interface IPlayerRecordsRepository
{
    /// <summary>
    /// Get a record from a player in a specific map.
    /// </summary>
    /// <param name="player">The player that has the record.</param>
    /// <param name="map">The map to get the record from.</param>
    /// <returns></returns>
    public Task<DbPlayerRecord?> GetRecordAsync(IPlayer player, IMap map);
    
    /// <summary>
    /// Add a new record to the database.
    /// </summary>
    /// <param name="record">The record to add.</param>
    /// <returns></returns>
    public Task<DbPlayerRecord> InsertRecordAsync(IPlayer player, IMap map, int score, IEnumerable<int> checkpoints);

    public Task DeleteRecordAsync(IPlayer player, IMap map);
    public Task DeleteRecordAsync(IPlayerRecord record);

    public Task<DbPlayerRecord[]> GetRecordsOfMapAsync(long mapId);
}
