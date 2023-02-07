using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Modules.Official.MatchManagerModule.Events;

public enum FlowControlEvent
{
    [Identifier(Name = "MatchManager.FlowControl.MapSkipped")]
    MapSkipped,
    
    [Identifier(Name = "MatchManager.FlowControl.ForcedRoundEnd")]
    ForcedRoundEnd,
    
    [Identifier(Name = "MatchManager.FlowControl.MatchRestarted")]
    MatchRestarted
}
