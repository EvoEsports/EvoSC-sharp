using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.MapListModule.Interfaces.Models;

namespace EvoSC.Modules.Official.MapListModule.Models;

public class MapListMapDto : IMapListMapDto
{
    public IMap Map { get; init; }
}
