using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.MapListModule.Interfaces.Models;

public interface IMapList
{
    public IEnumerable<IMap> Maps { get; }

    public void Add(IMap map);
    public void Remove(IMap map);
}
