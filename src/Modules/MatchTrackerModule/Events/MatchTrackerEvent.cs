using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Modules.Official.MatchTrackerModule.Events;

public enum MatchTrackerEvent
{
    [Identifier(Name = "MatchManager.MatchTracker.StateTracked")]
    StateTracked,
}
