namespace EvoSC.Modules.Official.MapListModule.Interfaces.Models;

/// <summary>
/// Holds information about a favorite map of a player.
/// </summary>
public interface IMapFavorite
{
    /// <summary>
    /// ID of the map that was favorited.
    /// </summary>
    public long MapId { get; }

    /// <summary>
    /// ID of the player that favorited the map.
    /// </summary>
    public long PlayerId { get; }
}
