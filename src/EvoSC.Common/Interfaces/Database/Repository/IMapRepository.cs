using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Models.Maps;

namespace EvoSC.Common.Interfaces.Database.Repository;

/// <summary>
/// Methods for map actions against the database.
/// </summary>
public interface IMapRepository
{
    /// <summary>
    /// Gets a map from the database based on the provided ID.
    /// </summary>
    /// <param name="id">The maps database ID.</param>
    /// <returns>a map if it exists in the database.</returns>
    public Task<IMap?> GetMapByIdAsync(long id);

    /// <summary>
    /// Get all maps in the database.
    /// </summary>
    /// <returns></returns>
    public Task<IMap[]> GetMapsAsync();
    
    /// <summary>
    /// Gets a map from the database based on the provided UID.
    /// </summary>
    /// <param name="uid">The maps unique ID.</param>
    /// <returns></returns>
    public Task<IMap?> GetMapByUidAsync(string uid);
    
    /// <summary>
    /// Adds a map to the database.
    /// </summary>
    /// <param name="map">Metadata from the map provider.</param>
    /// <param name="author">The map authors UID.</param>
    /// <param name="filePath">The filepath where the map is stored.</param>
    /// <returns></returns>
    public Task<IMap> AddMapAsync(MapMetadata map, IPlayer author, string filePath);

    /// <summary>
    /// Add details about a map to the database.
    /// </summary>
    /// <param name="mapDetails">Map details.</param>
    /// <param name="map">Map associated with these details.</param>
    /// <returns></returns>
    public Task<IMapDetails> AddMapDetailsAsync(IMapDetails mapDetails, IMap map);
    
    /// <summary>
    /// Updates an already existing map.
    /// </summary>
    /// <param name="mapId"></param>
    /// <param name="map"></param>
    /// <returns></returns>
    public Task<IMap> UpdateMapAsync(long mapId, MapMetadata map);
    
    /// <summary>
    /// Removes a map from the database.
    /// </summary>
    /// <param name="id">The maps database ID.</param>
    /// <returns></returns>
    public Task RemoveMapAsync(long id);

    /// <summary>
    /// Gets a map from the database based on the external provider ID.
    /// </summary>
    /// <returns></returns>
    public Task<IMap?> GetMapByExternalIdAsync(string id);

    public Task<IEnumerable<IMap>> GetMapsByUidAsync(IEnumerable<string> mapUids);
}
