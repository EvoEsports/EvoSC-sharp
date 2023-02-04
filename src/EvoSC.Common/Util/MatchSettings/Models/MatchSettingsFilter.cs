namespace EvoSC.Common.Util.MatchSettings.Models;

/// <summary>
/// Represents the "filter" section of a match settings file.
/// </summary>
public class MatchSettingsFilter
{
    /// <summary>
    /// Whether the server is available on LAN.
    /// </summary>
    public bool IsLan { get; set; }
    
    /// <summary>
    /// Whether the server is available to the internet.
    /// </summary>
    public bool IsInternet { get; set; }
    
    /// <summary>
    /// Whether this is solo mode.
    /// </summary>
    public bool IsSolo { get; set; }
    
    /// <summary>
    /// Whether we use hotseat mode or not.
    /// </summary>
    public bool IsHotseat { get; set; }
    
    /// <summary>
    /// The map sort index.
    /// </summary>
    public int SortIndex { get; set; }
    
    /// <summary>
    /// Whether to randomize the map order or not.
    /// </summary>
    public bool RandomMapOrder { get; set; }
}
