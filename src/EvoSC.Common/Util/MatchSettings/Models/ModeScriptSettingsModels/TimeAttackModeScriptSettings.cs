using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Common.Util.MatchSettings.Models.ModeScriptSettingsModels;

[ScriptSettingsFor(DefaultModeScriptName.TimeAttack)]
public class TimeAttackModeScriptSettings : ModeScriptSettings
{
    protected int ChatTime { get; set; }
    protected bool DecoImageUrlCheckpoint { get; set; }
    protected bool DecoImageUrlDecalSponsor4X1 { get; set; }
    protected bool DecoImageUrlScreen16X1 { get; set; }
    protected bool DecoImageUrlScreen16X9 { get; set; }
    protected bool DecoImageUrlScreen8X1 { get; set; }
    protected bool DecoImageUrlWhoAmIUrl { get; set; }
    protected bool DelayBeforeNextMap { get; set; }
    protected bool ForceLapsNb { get; set; }
    protected bool InfiniteLaps { get; set; }
    protected bool IsChannelServer { get; set; }
    protected bool IsSplitScreen { get; set; }
    protected bool NeutralEmblemUrl { get; set; }
    protected bool RespawnBehaviour { get; set; }
    protected bool ScriptEnvironment { get; set; }
    protected bool SeasonIds { get; set; }
    protected bool SynchronizePlayersAtMapStart { get; set; }
    protected bool SynchronizePlayersAtRoundStart { get; set; }
    protected bool TimeLimit { get; set; }
    protected bool TrustClientSimu { get; set; }
    protected bool UseClublinks { get; set; }
    protected bool UseClublinksSponsors { get; set; }
    protected bool UseCrudeExtrapolation { get; set; }
    protected bool WarmUpDuration { get; set; }
    protected bool WarmUpNb { get; set; }
    protected bool WarmUpTimeout { get; set; }
}
