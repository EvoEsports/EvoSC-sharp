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
    /// Add a map to a player's favorites, or remove it if it already is one.
    /// </summary>
    /// <param name="player">The player to update favorites for.</param>
    /// <param name="mapUid">UID of the map.</param>
    /// <returns>True if the map is now a favorite.</returns>
    public Task<bool> ToggleFavoriteAsync(IPlayer player, string mapUid);

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
    /// Disable a map if it is enabled, otherwise enable it. Disabling removes the map from the
    /// map rotation and hides it from players that can't toggle maps. Enabling adds it back to the rotation.
    /// </summary>
    /// <param name="actor">Player that toggled the map.</param>
    /// <param name="mapUid">UID of the map.</param>
    /// <returns>True if the map is now enabled, false if disabled, null if the map was not found or it failed.</returns>
    public Task<bool?> ToggleMapEnabledAsync(IPlayer actor, string mapUid);

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
