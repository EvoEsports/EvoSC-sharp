using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.PlayerRecords.Interfaces.Models;

namespace EvoSC.Modules.Official.LocalRecordsModule.Interfaces.Services;

public interface ILocalRecordsService
{
    /// <summary>
    /// Get all local records of the current map on the server.
    /// </summary>
    /// <returns></returns>
    public Task<ILocalRecord[]> GetLocalsOfCurrentMapAsync();
    
    /// <summary>
    /// Show/Update the local records widget to a player.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public Task ShowWidgetAsync(IPlayer player);
    
    /// <summary>
    /// Show/Update the local records widget to all players.
    /// </summary>
    /// <returns></returns>
    public Task ShowWidgetToAllAsync();
    
    /// <summary>
    /// Update a player's local record for the current map.
    /// </summary>
    /// <param name="record"></param>
    /// <returns></returns>
    public Task UpdatePbAsync(IPlayerRecord record);

    /// <summary>
    /// Remove all local records on all maps, and add them back based on the
    /// registered PBs of players.
    /// </summary>
    /// <returns></returns>
    public Task ResetLocalRecordsAsync();
}
