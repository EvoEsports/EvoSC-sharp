using EvoSC.Common.Database.Models.Maps;
using EvoSC.Common.Database.Models.Player;
using EvoSC.Common.Models;

namespace EvoSC.Common.Interfaces.Repository;

public interface IMapRepository
{
    public Task<DbMap?> GetMapById(long id);
    public Task<DbMap?> GetMapByUid(string uid);
    public Task<DbMap> AddMap(Map map, DbPlayer author, DbPlayer actor, string filePath);
    public Task<DbMap> UpdateMap(long mapId, Map map);
    public Task RemoveMap(Map map);
}
