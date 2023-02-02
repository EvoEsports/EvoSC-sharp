using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Common.Util.MatchSettings.Models.ModeScriptSettingsModels;

[ScriptSettingsFor(DefaultModeScriptName.TimeAttack)]
public class TimeAttackModeScriptSettings : ModeScriptSettings
{
    public new int ChatTime { get; set; }
}
