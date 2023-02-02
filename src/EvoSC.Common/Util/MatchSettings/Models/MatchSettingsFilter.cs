namespace EvoSC.Common.Util.MatchSettings.Models;

public class MatchSettingsFilter
{
    public bool IsLan { get; set; }
    public bool IsInternet { get; set; }
    public bool IsSolo { get; set; }
    public bool IsHotseat { get; set; }
    public int SortIndex { get; set; }
    public bool RandomMapOrder { get; set; }
}
