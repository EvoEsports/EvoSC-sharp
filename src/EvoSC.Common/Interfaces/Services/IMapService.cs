using EvoSC.Common.Database.Models;
using EvoSC.Common.Models;

namespace EvoSC.Common.Interfaces.Services;

public interface IMapService
{
    public Task<DbMap?> GetMapById(int id);
    public Task<DbMap?> GetMapByUid(string uid);
    public Task<DbMap> AddMap(Stream mapStream, Map map);
    public Task<IEnumerable<DbMap>> AddMaps(List<Map> maps);
    public Task<DbMap> UpdateMap(Map map);
    public Task RemoveMap(string mapUid);
}
