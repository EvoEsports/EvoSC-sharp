using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Common.Util.MatchSettings.Models;

public class MatchSettingsMapInfo
{
    public string FileName { get; set; }
    public string? Uid { get; set; }
    
    public MatchSettingsMapInfo(){}

    public MatchSettingsMapInfo(IMap map)
    {
        FileName = map.FilePath;
        Uid = map.Uid;
    }
}
