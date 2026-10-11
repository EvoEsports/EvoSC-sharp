using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.MapListModule.Interfaces.Models;
using EvoSC.Modules.Official.PlayerRecords.Interfaces.Models;

namespace EvoSC.Modules.Official.MapListModule.Models;

public class MapListMap : IMapListMap
{
    public required IMap Map { get; init; }
    public required IEnumerable<IMapTag> Tags { get; init; }
    public required IEnumerable<IPlayerRecord?> Records { get; init; }
}
