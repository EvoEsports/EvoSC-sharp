using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.PlayerRecords.Database.Models;
using EvoSC.Modules.Official.PlayerRecords.Interfaces.Models;

namespace EvoSC.Modules.Official.PlayerRecords.Interfaces;

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
    /// Update a record in the database.
    /// </summary>
    /// <param name="record">The record to update with new information.</param>
    /// <returns></returns>
    public Task UpdateRecordAsync(DbPlayerRecord record);
    
    /// <summary>
    /// Add a new record to the database.
    /// </summary>
    /// <param name="record">The record to add.</param>
    /// <returns></returns>
    public Task<DbPlayerRecord> InsertRecordAsync(IPlayer player, IMap map, int score, IEnumerable<int> checkpoints);

    public Task DeleteRecordAsync(IPlayer player, IMap map);
    public Task DeleteRecordAsync(IPlayerRecord record);
}
