using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.MapListModule.Interfaces.Models;

namespace EvoSC.Modules.Official.MapListModule.Models;

public class MapListMapDto : IMapListMapDto
{
    public IParsedMap Map { get; init; }
}
