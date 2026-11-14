using EvoSC.Modules.Official.MapListModule.Database.Models;

namespace EvoSC.Modules.Official.MapListModule.Interfaces.Database;

public interface IMapListRepository
{
    /// <summary>
    /// Get the IDs of all maps a player has favorited.
    /// </summary>
    public Task<HashSet<long>> GetFavoriteMapIdsAsync(long playerId);

    /// <summary>
    /// Set whether a map is a favorite of a player. Setting the current state again has no effect.
    /// </summary>
    public Task SetFavoriteAsync(long playerId, long mapId, bool favorite);

    /// <summary>
    /// Get all existing tags.
    /// </summary>
    public Task<IReadOnlyList<DbMapTag>> GetAllTagsAsync();

    /// <summary>
    /// Get the tags of all maps, keyed by map ID.
    /// </summary>
    public Task<Dictionary<long, List<DbMapTag>>> GetTagsByMapAsync();

    /// <summary>
    /// Add a tag to a map. The tag is created if it does not exist.
    /// </summary>
    public Task AddTagToMapAsync(long mapId, string tagName);

    /// <summary>
    /// Remove a tag from a map. Tags that are no longer used are deleted.
    /// </summary>
    /// <returns>True if the map had the tag.</returns>
    public Task<bool> RemoveTagFromMapAsync(long mapId, string tagName);
}
