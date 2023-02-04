namespace EvoSC.Common.Util.MatchSettings.Models;

public class MatchSettingsGameInfos
{
    public int GameMode { get; set; }
    public int ChatTime { get; set; }
    public int FinishTimeout { get; set; }
    public bool AllWarmupDuration { get; set; }
    public bool DisableRespawn { get; set; }
    public bool ForceShowAllOpponents { get; set; }
    public required string ScriptName { get; set; }
}
