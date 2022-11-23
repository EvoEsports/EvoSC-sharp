using EvoSC.Common.Database.Models;
using EvoSC.Common.Database.Models.Maps;
using EvoSC.Common.Models;

namespace EvoSC.Common.Interfaces.Services;

public interface IMapService
{
    public Task<DbMap?> GetMapById(int id);
    public Task<DbMap?> GetMapByUid(string uid);
    
    /// <summary>
    /// Adds a map to the database and to storage. If the map already exists and the passed map is newer than the existing one, the existing one will be overwritten.
    /// </summary>
    /// <param name="mapStream">A stream of the map file.</param>
    /// <param name="map">The map DTO to save the map info to the database.</param>
    /// <param name="player">The player who added the map to the server.</param>
    /// <exception cref="DuplicateNameException">Thrown if the map already exists within the database.</exception>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task<DbMap> AddMap(Stream mapStream, Map map);
    public Task<IEnumerable<DbMap>> AddMaps(List<Map> maps);
    public Task RemoveMap(string mapUid);
}
