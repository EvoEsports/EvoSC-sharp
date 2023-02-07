using EvoSC.Common.Util.MatchSettings.Attributes;

namespace EvoSC.Common.Util.MatchSettings.Models.ModeScriptSettingsModels;

[ScriptSettingsFor(DefaultModeScriptName.TmwtTeams)]
public class TmwtTeamsModeScriptSettings : ModeScriptSettings
{
    public new  int? ChatTime { get; set; }
    public new  bool? UseClublinks { get; set; }
    public new  bool UseClublinksSponsors { get; set; }
    public new  string? NeutralEmblemUrl { get; set; }
    public new  string? ScriptEnvironment { get; set; }
    public new  bool? IsChannelServer { get; set; }
    public new  int? DelayBeforeNextMap { get; set; }
    public new  int? ForceLapsNb { get; set; }
    public new  bool? InfiniteLaps { get; set; }
    public new  string? SeasonIds { get; set; }
    public new  bool? IsSplitScreen { get; set; }
    public new  string? DecoImageUrlWhoAmIUrl { get; set; }
    public new  string? DecoImageUrlCheckpoint { get; set; }
    public new  string? DecoImageUrlDecalSponsor4X1 { get; set; }
    public new  string? DecoImageUrlScreen16X9 { get; set; }
    public new  string? DecoImageUrlScreen8X1 { get; set; }
    public new  string? DecoImageUrlScreen16X1 { get; set; }
    public new  bool? TrustClientSimu { get; set; }
    public new  bool? UseCrudeExtrapolation { get; set; }
    public new  bool? SynchronizePlayersAtMapStart { get; set; }
    public new  bool? DisableGoToMap { get; set; }
    public new  string? PointsRepartition { get; set; }
    public new  bool? SynchronizePlayersAtRoundStart { get; set; }
    public new  int? MapPointsLimit { get; set; }
    public new  int? MatchPointsLimit { get; set; }
    public new  int? FinishTimeout { get; set; }
    public new  int? WarmUpNb { get; set; }
    public new  int? WarmUpDuration { get; set; }
    public new  int? WarmUpTimeout { get; set; }
    public new  string? MatchInfo { get; set; }
    public new  string? TeamsUrl { get; set; }
    public new  string? ForceRoadSpectatorsNb { get; set; }
    public new  bool? EnablePreMatch { get; set; }
    public new  bool? EarlyEndMatchCallback { get; set; }
    public new  bool? EnableDossardColor { get; set; }
}
