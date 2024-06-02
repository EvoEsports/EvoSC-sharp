using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.MapListModule.Interfaces.Models;

/// <summary>
/// Holds information about a favorite map of a player.
/// </summary>
public interface IMapFavorite
{
    /// <summary>
    /// The map that was favorited.
    /// </summary>
    public IMap Map { get; }
    
    /// <summary>
    /// The player that favorited the map.
    /// </summary>
    public IPlayer Player { get; }
}
