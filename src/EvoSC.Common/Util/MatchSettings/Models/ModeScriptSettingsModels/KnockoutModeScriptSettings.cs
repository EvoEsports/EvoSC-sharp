using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Common.Util.MatchSettings.Models.ModeScriptSettingsModels;

[ScriptSettingsFor(DefaultModeScriptName.Knockout)]
public class KnockoutModeScriptSettings : ModeScriptSettings
{
    public new int ChatTime { get; set; }
    public new bool DecoImageUrlCheckpoint { get; set; }
    public new bool DecoImageUrlDecalSponsor4X1 { get; set; }
    public new bool DecoImageUrlScreen16X1 { get; set; }
    public new bool DecoImageUrlScreen16X9 { get; set; }
    public new bool DecoImageUrlScreen8X1 { get; set; }
    public new bool DecoImageUrlWhoAmIUrl { get; set; }
    public new bool DelayBeforeNextMap { get; set; }
    public new bool EarlyEndMatchCallback { get; set; }
    public new bool EliminatedPlayerNbRanks { get; set; }
    public new bool FinishTimeout { get; set; }
    public new bool ForceLapsNb { get; set; }
    public new bool InfiniteLaps { get; set; }
    public new bool IsChannelServer { get; set; }
    public new bool IsSplitScreen { get; set; }
    public new bool MatchPosition { get; set; }
    public new bool NeutralEmblemUrl { get; set; }
    public new bool PointsRepartition { get; set; }
    public new bool RespawnBehaviour { get; set; }
    public new bool RoundsPerMap { get; set; }
    public new bool RoundsWithoutElimination { get; set; }
    public new bool ScriptEnvironment { get; set; }
    public new bool SeasonIds { get; set; }
    public new bool SynchronizePlayersAtMapStart { get; set; }
    public new bool SynchronizePlayersAtRoundStart { get; set; }
    public new bool TrustClientSimu { get; set; }
    public new bool UseClublinks { get; set; }
    public new bool UseClublinksSponsors { get; set; }
    public new bool UseCrudeExtrapolation { get; set; }
    public new bool WarmUpDuration { get; set; }
    public new bool WarmUpNb { get; set; }
    public new bool WarmUpTimeout { get; set; }
}
