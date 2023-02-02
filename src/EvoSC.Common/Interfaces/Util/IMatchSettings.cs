using EvoSC.Common.Util.MatchSettings;
using EvoSC.Common.Util.MatchSettings.Models;

namespace EvoSC.Common.Interfaces.Util;

public interface IMatchSettings
{
    /// <summary>
    /// Game information config.
    /// </summary>
    public MatchSettingsGameInfos GameInfos { get; set; }
    
    /// <summary>
    /// Match filter options.
    /// </summary>
    public MatchSettingsFilter Filter { get; set; }
    
    /// <summary>
    /// Mode script options.
    /// </summary>
    public ModeScriptSettings ModeScriptSettings { get; set; }

    /// <summary>
    /// The index to start in the map list.
    /// </summary>
    public int StartIndex { get; set; }
}
