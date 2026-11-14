using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.PlayerRecords.Interfaces.Models;

namespace EvoSC.Modules.Official.MapListModule.Interfaces.Models;

public interface IMapListMap
{
    /// <summary>
    /// The map object.
    /// </summary>
    public IMap Map { get; }

    /// <summary>
    /// Tags assigned to the map.
    /// </summary>
    public IEnumerable<IMapTag> Tags { get; }

    /// <summary>
    /// Records a single player has driven on the map 
    /// </summary>
    public IEnumerable<IPlayerRecord?> Records { get; }

    /// <summary>
    /// Whether the player has marked this map as a favorite.
    /// </summary>
    public bool IsFavorite { get; }

    /// <summary>
    /// Whether the map is enabled. Disabled maps are not in the map rotation.
    /// </summary>
    public bool IsEnabled => Map.Enabled;

    /// <summary>
    /// Whether the player has a time on the map.
    /// </summary>
    public bool HasPb => Records.Any();

    /// <summary>
    /// The best record driven by the player.
    /// </summary>
    public IPlayerRecord? Pb => Records.FirstOrDefault();
}
