using System.Data;
using EvoSC.Common.Database.Models.Maps;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Models;

namespace EvoSC.Common.Interfaces.Services;

public interface IMapService
{
    public Task<IMap?> GetMapById(int id);
    public Task<IMap?> GetMapByUid(string uid);
    
    /// <summary>
    /// Adds a map to the database and to storage. If the map already exists and the passed map is newer than the existing one, the existing one will be overwritten.
    /// </summary>
    /// <param name="mapStream">An object containing the map file and the map information.</param>
    /// <param name="actor">The player who added the map to the server.</param>
    /// <exception cref="DuplicateNameException">Thrown if the map already exists within the database.</exception>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task<IMap> AddMap(MapStream mapStream, IPlayer actor);
    public Task<IEnumerable<IMap>> AddMaps(List<MapStream> mapObjects, IPlayer actor);
    public Task RemoveMap(string mapUid);
}
