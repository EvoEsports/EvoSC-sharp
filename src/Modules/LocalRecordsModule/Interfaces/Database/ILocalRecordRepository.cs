using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.LocalRecordsModule.Database.Models;
using EvoSC.Modules.Official.PlayerRecords.Interfaces.Models;

namespace EvoSC.Modules.Official.LocalRecordsModule.Interfaces.Database;

public interface ILocalRecordRepository
{
    /// <summary>
    /// Get all local records from a map by its ID.
    /// </summary>
    /// <param name="mapId">ID of the map to get local records from.</param>
    /// <returns></returns>
    public Task<IEnumerable<DbLocalRecord>> GetLocalRecordsOfMapByIdAsync(long mapId);
    
    /// <summary>
    /// Add a new local record, or update it if its better than existing one.
    /// </summary>
    /// <param name="map">Map to add local record to.</param>
    /// <param name="record">The record to add.</param>
    /// <returns></returns>
    public Task<DbLocalRecord?> AddOrUpdateRecordAsync(IMap map, IPlayerRecord record);
    
    /// <summary>
    /// Add multiple local records to a map.
    /// </summary>
    /// <param name="map">Map to add local records to.</param>
    /// <param name="records">Records to add.</param>
    /// <returns></returns>
    public Task AddRecordsAsync(IMap map, IEnumerable<IPlayerRecord> records);
    
    /// <summary>
    /// Recalculate positions of all local records in a map. It will
    /// also remove local records that does not fit within the configured
    /// top N local records.
    /// </summary>
    /// <param name="map">Map to re-calculate local records for.</param>
    /// <returns></returns>
    public Task<DbLocalRecord[]> RecalculatePositionsOfMapAsync(IMap map);
    
    /// <summary>
    /// Get local records from a player.
    /// </summary>
    /// <param name="player">Player to get local records from.</param>
    /// <returns></returns>
    public Task<IEnumerable<DbLocalRecord>> GetRecordsByPlayerAsync(IPlayer player);
    
    /// <summary>
    /// Get the local record of a player for a map.
    /// </summary>
    /// <param name="player">The player to get the local record of.</param>
    /// <param name="map">The map to get the local record for.</param>
    /// <returns></returns>
    public Task<DbLocalRecord?> GetRecordOfPlayerInMapAsync(IPlayer player, IMap map);
    
    /// <summary>
    /// Delete a local record for a player in a map.
    /// </summary>
    /// <param name="player">The player that has the local record.</param>
    /// <param name="map">The map that contains the local record.</param>
    /// <returns></returns>
    public Task DeleteRecordAsync(IPlayer player, IMap map);
    
    /// <summary>
    /// Delete a local record.
    /// </summary>
    /// <param name="localRecord">The local record to remove.</param>
    /// <returns></returns>
    public Task DeleteRecordAsync(ILocalRecord localRecord);
    
    /// <summary>
    /// Delete all local records from a map.
    /// </summary>
    /// <param name="map">The map to clear all local records from.</param>
    /// <returns></returns>
    public Task DeleteRecordsAsync(IMap map);
}
