using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Common.Util.MatchSettings.Models.ModeScriptSettingsModels;

[ScriptSettingsFor(DefaultModeScriptName.Champion)]
public class ChampionModeScriptSettings : ModeScriptSettings
{
    public new int BestLapBonusPoints { get; set; }
    public new int ChatTime { get; set; }
    public new bool DecoImageUrlCheckpoint { get; set; }
    public new bool DecoImageUrlDecalSponsor4X1 { get; set; }
    public new bool DecoImageUrlScreen16X1 { get; set; }
    public new bool DecoImageUrlScreen16X9 { get; set; }
    public new bool DecoImageUrlScreen8X1 { get; set; }
    public new bool DecoImageUrlWhoAmIUrl { get; set; }
    public new bool DelayBeforeNextMap { get; set; }
    public new bool DisableGiveUp { get; set; }
    public new bool EarlyEndMatchCallback { get; set; }
    public new bool EndRoundPostScoreUpdateDuration { get; set; }
    public new bool EndRoundPreScoreUpdateDuration { get; set; }
    public new bool FinishTimeout { get; set; }
    public new bool ForceLapsNb { get; set; }
    public new bool ForceWinnersNb { get; set; }
    public new bool InfiniteLaps { get; set; }
    public new bool IsChannelServer { get; set; }
    public new bool IsSplitScreen { get; set; }
    public new bool NeutralEmblemUrl { get; set; }
    public new bool PauseBeforeRoundNb { get; set; }
    public new bool PauseDuration { get; set; }
    public new bool PointsLimit { get; set; }
    public new bool PointsRepartition { get; set; }
    public new bool RespawnBehaviour { get; set; }
    public new bool RoundsLimit { get; set; }
    public new bool RoundsWithAPhaseChange { get; set; }
    public new bool ScriptEnvironment { get; set; }
    public new bool SeasonIds { get; set; }
    public new bool SynchronizePlayersAtMapStart { get; set; }
    public new bool SynchronizePlayersAtRoundStart { get; set; }
    public new bool TimeLimit { get; set; }
    public new bool TimeOutPlayersNumber { get; set; }
    public new bool TrustClientSimu { get; set; }
    public new bool UseClublinks { get; set; }
    public new bool UseClublinksSponsors { get; set; }
    public new bool UseCrudeExtrapolation { get; set; }
    public new bool UseTieBreak { get; set; }
    public new bool WarmUpDuration { get; set; }
    public new bool WarmUpNb { get; set; }
    public new bool WarmUpTimeout { get; set; }
}
