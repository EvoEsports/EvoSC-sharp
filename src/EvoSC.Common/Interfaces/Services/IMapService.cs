using System.Data;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Models.Maps;

namespace EvoSC.Common.Interfaces.Services;

public interface IMapService
{
    /// <summary>
    /// Gets a map.
    /// </summary>
    /// <param name="id">The database ID.</param>
    /// <returns></returns>
    public Task<IMap?> GetMapByIdAsync(long id);
    
    /// <summary>
    /// Gets a map.
    /// </summary>
    /// <param name="uid">The maps unique identifier.</param>
    /// <returns></returns>
    public Task<IMap?> GetMapByUidAsync(string uid);
    
    /// <summary>
    /// Adds a map to the server. If the map already exists and the passed map is newer than the
    /// existing one, the existing one will be overwritten.
    /// </summary>
    /// <param name="mapStream">An object containing the map file and the map metadata.</param>
    /// <exception cref="DuplicateNameException">Thrown if the map already exists within the database.</exception>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task<IMap> AddMapAsync(MapStream mapStream);
    
    /// <summary>
    /// Add several maps to the server. Useful for adding mappacks.
    /// </summary>
    /// <param name="mapStreams">A list of objects containing the mapfile and the map metadata.</param>
    /// <returns></returns>
    public Task<IEnumerable<IMap>> AddMapsAsync(List<MapStream> mapStreams);
    
    /// <summary>
    /// Removes a map from the server.
    /// </summary>
    /// <param name="mapId">The maps ID in the database.</param>
    /// <returns></returns>
    public Task RemoveMapAsync(long mapId);

    /// <summary>
    /// Add all maps in the current map list to the database if they don't already exist.
    /// </summary>
    /// <returns></returns>
    public Task AddCurrentMapListAsync();
    
    /// <summary>
    /// Get the current map on the server.
    /// </summary>
    /// <returns></returns>
    public Task<IMap> GetOrAddCurrentMapAsync();
}
