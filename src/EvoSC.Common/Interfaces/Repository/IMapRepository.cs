using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Models;

namespace EvoSC.Common.Interfaces.Repository;

public interface IMapRepository
{
    public Task<IMap?> GetMapById(long id);
    
    public Task<IMap?> GetMapByUid(string uid);
    
    public Task<IMap> AddMap(MapMetadata map, IPlayer player, string filePath);
    
    public Task<IMap> UpdateMap(long mapId, MapMetadata map);
    
    public Task RemoveMap(long id);
}
