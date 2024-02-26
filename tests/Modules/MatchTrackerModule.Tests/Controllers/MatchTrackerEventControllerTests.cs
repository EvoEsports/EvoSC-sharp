using EvoSC.Common.Models;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.MatchTrackerModule.Config;
using EvoSC.Modules.Official.MatchTrackerModule.Controllers;
using EvoSC.Modules.Official.MatchTrackerModule.Interfaces;
using EvoSC.Testing.Controllers;
using NSubstitute;

namespace EvoSC.Modules.Official.MatchTrackerModule.Tests.Controllers;

public class MatchTrackerEventControllerTests : EventControllerTestBase<MatchTrackerEventController>
{
    private readonly ITrackerSettings _settings = Substitute.For<ITrackerSettings>();
    private readonly IMatchTracker _tracker = Substitute.For<IMatchTracker>();

    public MatchTrackerEventControllerTests()
    {
        InitMock(_settings, _tracker);
    }

    [Fact]
    public async Task Scores_Report_Is_Tracked()
    {
        var scoresArgs = new ScoresEventArgs
        {
            Section = ModeScriptSection.Undefined,
            UseTeams = false,
            WinnerTeam = 0,
            WinnerPlayer = null,
            Teams = null,
            Players = null
        };

        await Controller.OnScoresAsync(null, scoresArgs);
        
        await _tracker.Received(1).TrackScoresAsync(scoresArgs);
    }

    [Theory]
    [InlineData(true, true)]
    [InlineData(false, false)]
    public async Task Begin_Match_Is_Tracked_Depending_On_Settings(bool automaticTracking, bool isTracked)
    {
        _settings.AutomaticTracking.Returns(automaticTracking);

        await Controller.OnBeginMatchAsync(null, EventArgs.Empty);

        await _tracker.Received(isTracked ? 1 : 0).BeginMatchAsync();
    }
}
