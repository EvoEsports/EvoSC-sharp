using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.MapListModule.Interfaces.Models;
using EvoSC.Modules.Official.MapListModule.Models;

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
    /// Open the map list for a player from scratch. The player's search, filters and sorting are reset first.
    /// Use <see cref="ShowMapListAsync"/> to redraw the list while keeping them.
    /// </summary>
    /// <param name="player">Player to open the maplist for.</param>
    /// <returns></returns>
    public Task OpenMapListAsync(IPlayer player);

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
    public Task ConfirmMapDeletionAsync(IPlayer player, IMap map);

    /// <summary>
    /// Get the maps of the current map list after applying search, filters and sorting.
    /// </summary>
    /// <param name="player">Player to get the maps for.</param>
    /// <param name="filter">Search, filter and sort options.</param>
    /// <returns></returns>
    public Task<MapListResult> GetFilteredMapsForPlayerAsync(IPlayer player, MapListFilter filter);

    /// <summary>
    /// Add a map to a player's favorites or remove it. Setting the current state again has no effect.
    /// </summary>
    /// <param name="player">The player to update favorites for.</param>
    /// <param name="mapUid">UID of the map.</param>
    /// <param name="favorite">True to make the map a favorite, false to remove it from the favorites.</param>
    /// <returns>False if the map was not found.</returns>
    public Task<bool> SetFavoriteAsync(IPlayer player, string mapUid, bool favorite);

    /// <summary>
    /// Add a tag to a map.
    /// </summary>
    /// <param name="mapUid">UID of the map.</param>
    /// <param name="tag">Name of the tag.</param>
    /// <returns>False if the map was not found or the tag name is invalid.</returns>
    public Task<bool> AddTagAsync(string mapUid, string tag);

    /// <summary>
    /// Remove a tag from a map.
    /// </summary>
    /// <param name="mapUid">UID of the map.</param>
    /// <param name="tag">Name of the tag.</param>
    /// <returns>False if the map or the tag was not found.</returns>
    public Task<bool> RemoveTagAsync(string mapUid, string tag);

    /// <summary>
    /// Enable or disable a map. Disabling removes the map from the map rotation and hides it from players
    /// that can't toggle maps. Enabling adds it back to the rotation. Setting the current state again has no effect.
    /// </summary>
    /// <param name="actor">Player that changed the map.</param>
    /// <param name="mapUid">UID of the map.</param>
    /// <param name="enabled">True to enable the map, false to disable it.</param>
    /// <returns>False if the map was not found or it failed.</returns>
    public Task<bool> SetMapEnabledAsync(IPlayer actor, string mapUid, bool enabled);

    /// <summary>
    /// Show the dialog for viewing and editing the tags of a map.
    /// </summary>
    /// <param name="player">Player to show the dialog to.</param>
    /// <param name="mapUid">UID of the map to edit tags for.</param>
    /// <returns></returns>
    public Task ShowTagsDialogAsync(IPlayer player, string mapUid);

    /// <summary>
    /// Show a read-only dialog listing all tags of a map.
    /// </summary>
    public Task ShowTagsViewDialogAsync(IPlayer player, string mapUid);

    /// <summary>
    /// Show the dialog for searching and adding maps from Trackmania Exchange.
    /// </summary>
    /// <param name="player">Player to show the dialog to.</param>
    /// <returns></returns>
    public Task ShowAddMapDialogAsync(IPlayer player);
}
