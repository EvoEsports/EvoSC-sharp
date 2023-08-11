using EvoSC.Modules.Official.MatchManagerModule.Interfaces.Models;

namespace EvoSC.Modules.Official.MatchManagerModule.Util;

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
