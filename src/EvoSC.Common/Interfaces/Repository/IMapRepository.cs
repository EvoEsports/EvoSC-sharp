using EvoSC.Common.Database.Models.Maps;
using EvoSC.Common.Database.Models.Player;
using EvoSC.Common.Models;

namespace EvoSC.Common.Interfaces.Repository;

public interface IMapRepository
{
    public Task<DbMap?> GetMapById(int id);
    public Task<DbMap?> GetMapByUid(string uid);
    public Task<DbMap> AddMap(Map map, DbPlayer player, string filePath);
    public Task<DbMap> UpdateMap(int mapId, Map map);
    public Task RemoveMap(Map map);
}
