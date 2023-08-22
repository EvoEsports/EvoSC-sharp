using EvoSC.Modules.Official.MatchTrackerModule.Interfaces.Models;

namespace EvoSC.Modules.Official.MatchTrackerModule.Util;

public static class MatchStateUtils
{
    public static object CastToSuperClass(this IMatchState state)
    {
        if (state is IScoresMatchState matchState)
        {
            return matchState;
        }

        return state as IMatchState;
    }
}
