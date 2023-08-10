using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Modules.Official.MatchManagerModule.Events;

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
