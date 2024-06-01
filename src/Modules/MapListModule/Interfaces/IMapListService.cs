using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.MapListModule.Interfaces.Models;

namespace EvoSC.Modules.Official.MapListModule.Interfaces;

public interface IMapListService
{
    /// <summary>
    /// Get a list of available maps in the current match settings, along with
    /// information of a specific player for each map (such as records).
    /// </summary>
    /// <param name="player">Player to get maps for.</param>
    /// <returns></returns>
    public Task<IEnumerable<IMapListMap>> GetCurrentMapsForPlayerAsync(IPlayer player);
    
    /// <summary>
    /// Delete a map in the current match settings.
    /// </summary>
    /// <param name="actor">Player that initiated the deletion.</param>
    /// <param name="mapUid">UID of the map to delete.</param>
    /// <returns></returns>
    public Task DeleteMapAsync(IPlayer actor, string mapUid);

    /// <summary>
    /// Show the map list UI to a player.
    /// </summary>
    /// <param name="player">Player to show the maplist to.</param>
    /// <returns></returns>
    public Task ShowMapListAsync(IPlayer player);
    
    /// <summary>
    /// Show the confirmation dialog for map deletion to a player. Calling this will
    /// automatically delete the map if the player confirms.
    /// </summary>
    /// <param name="player">Player that needs to confirm the map deletion.</param>
    /// <param name="map">Map to be deleted.</param>
    /// <returns></returns>
    public Task ConfirmMapDeletionsAsync(IPlayer player, IMap map);
}
