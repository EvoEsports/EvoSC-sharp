using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Common.Util.MatchSettings.Models;

/// <summary>
/// Represents a single map within a match settings file.
/// </summary>
public class MatchSettingsMapInfo
{
    /// <summary>
    /// The path/name of the map's file relative to the Maps directory.
    /// </summary>
    public string FileName { get; set; }
    
    /// <summary>
    /// The unique ID of the map.
    /// </summary>
    public string? Uid { get; set; }
    
    public MatchSettingsMapInfo(){}

    public MatchSettingsMapInfo(IMap map)
    {
        FileName = map.FilePath;
        Uid = map.Uid;
    }
}
