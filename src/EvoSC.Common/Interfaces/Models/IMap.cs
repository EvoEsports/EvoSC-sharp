using EvoSC.Common.Models.Maps;

namespace EvoSC.Common.Interfaces.Models;

/// <summary>
/// Represents a map.
/// </summary>
public interface IMap
{
    /// <summary>
    /// The maps ID.
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// The maps unique ID.
    /// </summary>
    public string Uid { get; set; }
    
    /// <summary>
    /// The maps name.
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// The map author.
    /// </summary>
    public IPlayer? Author { get; }
    
    /// <summary>
    /// Path to the map relative to the Maps directory.
    /// </summary>
    public string FilePath { get; set; }
    
    /// <summary>
    /// Whether the map is enabled or disabled.
    /// </summary>
    public bool Enabled { get; set; }
    
    /// <summary>
    /// The maps external ID. E.g. the TrackmaniaIo ID.
    /// </summary>
    public string ExternalId { get; set; }
    
    /// <summary>
    /// The maps external version. E.g. the "last updated" value of the TrackmaniaIo map.
    /// </summary>
    public DateTime? ExternalVersion { get; set; }
    
    /// <summary>
    /// The maps external provider. E.g. trackmania.io.
    /// </summary>
    public MapProviders? ExternalMapProvider { get; set; }
}
