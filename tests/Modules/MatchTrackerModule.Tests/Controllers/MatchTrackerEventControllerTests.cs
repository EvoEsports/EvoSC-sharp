using EvoSC.Common.Models;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.MatchTrackerModule.Config;
using EvoSC.Modules.Official.MatchTrackerModule.Controllers;
using EvoSC.Modules.Official.MatchTrackerModule.Interfaces;
using EvoSC.Testing.Controllers;
using Moq;

namespace EvoSC.Modules.Official.MatchTrackerModule.Tests.Controllers;

public class MatchTrackerEventControllerTests : EventControllerTestBase<MatchTrackerEventController>
{
    private Mock<ITrackerSettings> _settings = new();
    private Mock<IMatchTracker> _tracker = new();

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
        
        _tracker.Verify(m => m.TrackScoresAsync(scoresArgs), Times.Once);
    }

    [Theory]
    [InlineData(true, true)]
    [InlineData(false, false)]
    public async Task Begin_Match_Is_Tracked_Depending_On_Settings(bool automaticTracking, bool isTracked)
    {
        _settings.Setup(m => m.AutomaticTracking).Returns(automaticTracking);

        await Controller.OnBeginMatchAsync(null, EventArgs.Empty);

        var timesCalled = isTracked ? Times.Once() : Times.Never();
        
        _tracker.Verify(m => m.BeginMatchAsync(), timesCalled);
    }

    [Fact]
    public async Task Manual_Match_Tracking_Is_Disabled_If_Automatic_Tracking_Is_Enabled()
    {
        _settings.Setup(m => m.AutomaticTracking).Returns(true);

        await Controller.OnMatchStarted(null, null);
        
        _tracker.Verify(m => m.BeginMatchAsync(), Times.Never);
    }
    
    [Fact]
    public async Task Manual_Match_Tracking_Is_Triggered_If_Automatic_Tracking_Is_Disabled()
    {
        _settings.Setup(m => m.AutomaticTracking).Returns(false);

        await Controller.OnMatchStarted(null, null);
        
        _tracker.Verify(m => m.BeginMatchAsync(), Times.Once);
    }
}
