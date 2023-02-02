using System.ComponentModel;
using EvoSC.Common.Util.EnumIdentifier;
using EvoSC.Common.Util.MatchSettings.Attributes;

namespace EvoSC.Common.Util.MatchSettings;

public class ModeScriptSettings
{
    [ScriptSetting(Name = "S_BestLapBonusPoints")]
    [Description("Points bonus attributed to the player with the best lap")]
    [DefaultScriptSettingValue(OnMode = DefaultModeScriptName.Champion, Value = 2)]
    protected int BestLapBonusPoints { get; set; }

    [ScriptSetting(Name = "S_ChatTime")]
    [Description("Chat time at the end of a map or match")]
    [DefaultScriptSettingValue(OnMode = DefaultModeScriptName.Champion, Value = 30)]
    [DefaultScriptSettingValue(OnMode = DefaultModeScriptName.Cup, Value = 10)]
    [DefaultScriptSettingValue(OnMode = DefaultModeScriptName.Knockout, Value = 6)]
    [DefaultScriptSettingValue(OnMode = DefaultModeScriptName.Laps, Value = 10)]
    [DefaultScriptSettingValue(OnMode = DefaultModeScriptName.Teams, Value = 10)]
    [DefaultScriptSettingValue(OnMode = DefaultModeScriptName.TimeAttack, Value = 10)]
    [DefaultScriptSettingValue(OnMode = DefaultModeScriptName.Rounds, Value = 10)]
    protected int ChatTime { get; set; }
    
    [ScriptSetting(Name = "S_CumulatePoints")]
    [Description("Cumulate points earned by players to their team score")]
    [DefaultScriptSettingValue(OnMode = DefaultModeScriptName.Teams, Value = false)]
    protected bool CumulatePoints { get; set; }
}
