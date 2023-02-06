using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Util.MatchSettings.Models;

namespace EvoSC.Common.Interfaces.Util;

public interface IMatchSettings
{
    /// <summary>
    /// Game information config.
    /// </summary>
    public MatchSettingsGameInfos? GameInfos { get; set; }
    
    /// <summary>
    /// Match filter options.
    /// </summary>
    public MatchSettingsFilter? Filter { get; set; }
    
    /// <summary>
    /// Mode script options.
    /// </summary>
    public Dictionary<string, ModeScriptSettingInfo>? ModeScriptSettings { get; set; }
    
    /// <summary>
    /// The map list of this match settings.
    /// </summary>
    public List<IMap>? Maps { get; set; }

    /// <summary>
    /// The index to start in the map list.
    /// </summary>
    public int StartIndex { get; set; }
}
