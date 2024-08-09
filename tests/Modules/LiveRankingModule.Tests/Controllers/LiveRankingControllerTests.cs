using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Models;
using EvoSC.Common.Models.Callbacks;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.LiveRankingModule.Controllers;
using EvoSC.Modules.Official.LiveRankingModule.Interfaces;
using EvoSC.Testing.Controllers;
using GbxRemoteNet.Events;
using Moq;
using Xunit;

namespace EvoSC.Modules.Official.LiveRankingModule.Tests.Controllers;

public class LiveRankingControllerTests : ControllerMock<LiveRankingEventController, IEventControllerContext>
{
    private Mock<ILiveRankingService> _liveRankingService = new();

    public LiveRankingControllerTests()
    {
        InitMock(_liveRankingService);
    }

    [Fact]
    public async Task Detects_Mode_And_Requests_Scores_On_Begin_Map()
    {
        await Controller.OnBeginMapAsync(null, new MapGbxEventArgs());

        _liveRankingService.Verify(s => s.DetectModeAndRequestScoreAsync(), Times.Once);
    }

    [Theory]
    [InlineData(ModeScriptSection.Undefined, 1)]
    [InlineData(ModeScriptSection.EndRound, 1)]
    [InlineData(ModeScriptSection.PreEndRound, 0)]
    [InlineData(ModeScriptSection.EndMap, 0)]
    [InlineData(ModeScriptSection.EndMatchEarly, 0)]
    [InlineData(ModeScriptSection.EndMatch, 0)]
    public async Task Maps_Scores_For_Correct_Mode_Script_Sections(ModeScriptSection section, int expectedCallCount)
    {
        var scoresEventArgs = new ScoresEventArgs
        {
            Section = section,
            UseTeams = false,
            WinnerTeam = 0,
            WinnerPlayer = null,
            Teams = new List<TeamScore>(),
            Players = new List<PlayerScore?>()
        };

        await Controller.OnScoresAsync(null, scoresEventArgs);

        _liveRankingService.Verify(
            s => s.MapScoresAndSendWidgetAsync(scoresEventArgs),
            Times.Exactly(expectedCallCount)
        );
    }

    [Fact]
    public async Task Hides_Widget_On_Podium_Start()
    {
        await Controller.OnPodiumStartAsync(null, new PodiumEventArgs { Time = 0 });

        _liveRankingService.Verify(s => s.HideWidgetAsync(), Times.Once);
    }

    [Theory]
    [InlineData(true, false, 1)]
    [InlineData(true, true, 0)]
    [InlineData(false, true, 0)]
    [InlineData(false, false, 0)]
    public async Task Requests_Scores_For_Each_Finish_In_TimeAttack_Mode(bool isEndLap, bool isPointsBasedMode,
        int expectedCallCount)
    {
        var waypointEventArgs = new WayPointEventArgs
        {
            Login = "",
            AccountId = "",
            RaceTime = 0,
            LapTime = 0,
            CheckpointInRace = 0,
            CheckpointInLap = 0,
            IsEndRace = false,
            IsEndLap = isEndLap,
            CurrentRaceCheckpoints = new List<int>(),
            CurrentLapCheckpoints = new List<int>(),
            BlockId = "",
            Speed = 0,
            Time = 0
        };

        _liveRankingService.Setup(s => s.CurrentModeIsPointsBasedAsync())
            .Returns(Task.FromResult(isPointsBasedMode));

        await Controller.OnWayPointAsync(null, waypointEventArgs);

        _liveRankingService.Verify(
            s => s.RequestScoresAsync(),
            Times.Exactly(expectedCallCount)
        );
    }
}
