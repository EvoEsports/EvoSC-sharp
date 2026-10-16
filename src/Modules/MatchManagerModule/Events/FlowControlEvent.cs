using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Modules.Official.MatchManagerModule.Events;

/// <summary>
/// Exported so modules can subscribe to the match control events without a reference to this
/// module: both sides then agree on the same enum type, and therefore on the event names.
/// </summary>
[Export]
public enum FlowControlEvent
{
    [Identifier(Name = "MatchManager.MatchControl.MapSkipped")]
    MapSkipped,
    
    [Identifier(Name = "MatchManager.MatchControl.ForcedRoundEnd")]
    ForcedRoundEnd,
    
    [Identifier(Name = "MatchManager.MatchControl.MatchRestarted")]
    MatchRestarted,
    
    [Identifier(Name = "MatchManager.MatchControl.MatchStarted")]
    MatchStarted,
    
    [Identifier(Name = "MatchManager.MatchControl.MatchEnded")]
    MatchEnded
}
