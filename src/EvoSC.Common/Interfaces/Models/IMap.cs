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
    public IPlayer Author { get; set; }
    
    /// <summary>
    /// The maps TrackmaniaExchange ID.
    /// </summary>
    public long ManiaExchangeId { get; set; }
    
    /// <summary>
    /// The maps version in TrackmaniaExchange.
    /// </summary>
    public DateTime? ManiaExchangeVersion { get; set; }
    
    /// <summary>
    /// The maps TrackmaniaIO ID.
    /// </summary>
    public long TrackmaniaIoId { get; set; }
    
    /// <summary>
    /// The maps version in TrackmaniaIO.
    /// </summary>
    public DateTime? TrackmaniaIoVersion { get; set; }
}
