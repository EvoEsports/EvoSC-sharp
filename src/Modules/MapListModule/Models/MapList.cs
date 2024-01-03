using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.MapListModule.Interfaces.Models;

namespace EvoSC.Modules.Official.MapListModule.Models;

public class MapList : IMapList
{
    private readonly Dictionary<string, IMap> _maps = [];

    public IEnumerable<IMap> Maps => _maps.Values;
    
    public void Add(IMap map)
    {
        if (!_maps.TryAdd(map.Uid, map))
        {
            throw new InvalidOperationException($"Map with UID '{map.Uid}' already exists map list");
        }
    }

    public void Remove(IMap map)
    {
        if (!_maps.ContainsKey(map.Uid))
        {
            throw new InvalidOperationException($"Map with UID '{map.Uid}' does not exist in the map list");
        }

        _maps.Remove(map.Uid);
    }
}
