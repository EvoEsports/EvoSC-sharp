using EvoSC.Common.Models;
using EvoSC.Modules.Official.MatchTrackerModule.Interfaces.Models;
using EvoSC.Modules.Official.MatchTrackerModule.Models;
using EvoSC.Modules.Official.MatchTrackerModule.Util;

namespace EvoSC.Modules.Official.MatchTrackerModule.Tests.Util;

public class MatchStateUtilsTests
{
    [Fact]
    public void State_Is_Casted_To_ScoresMatchState()
    {
        IMatchState state = new ScoresMatchState
        {
            TimelineId = default,
            Status = MatchStatus.Unknown,
            Timestamp = default,
            Section = ModeScriptSection.Undefined,
            UseTeams = false,
            WinnerTeam = 0,
            WinnerPlayer = null,
            Teams = null,
            Players = null
        };

        var casted = state.CastToSuperClass();
        
        var intf = casted.GetType().GetInterface("IScoresMatchState");
        Assert.NotNull(intf);
    }

    interface IUnknownStateType : IMatchState
    {
        public int Test { get; }
    }

    class UnknownStateType : IUnknownStateType
    {
        public Guid TimelineId { get; }
        public MatchStatus Status { get; }
        public DateTime Timestamp { get; }
        public int Test { get; }
    }
}
