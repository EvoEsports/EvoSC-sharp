using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.MapListModule.Interfaces;

public class IMapList
{
    public IEnumerable<IMap> Maps { get; }
}
