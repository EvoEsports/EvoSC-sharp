using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.RoundRankingModule.Controllers;
using EvoSC.Modules.Official.RoundRankingModule.Interfaces;
using EvoSC.Testing.Controllers;
using Moq;

namespace EvoSC.Modules.Official.RoundRankingModule.Tests.Controllers;

public class RoundRankingEventControllerTests : ControllerMock<RoundRankingEventController, IEventControllerContext>
{
    private Mock<IRoundRankingService> _roundRankingService = new();

    public RoundRankingEventControllerTests()
    {
        InitMock(_roundRankingService);
    }

    [Fact]
    public async Task Consumes_Checkpoint_Data_On_Waypoint_Event()
    {
        const string AccountId = "*fakeplayer1*";
        var waypointEventArgs = new WayPointEventArgs
        {
            Login = AccountId,
            AccountId = AccountId,
            RaceTime = 1234,
            LapTime = 1234,
            CheckpointInRace = 1,
            CheckpointInLap = 1,
            IsEndRace = false,
            IsEndLap = false,
            CurrentRaceCheckpoints = [],
            CurrentLapCheckpoints = [],
            BlockId = "",
            Speed = 0,
            Time = 0
        };

        await Controller.OnWaypointAsync(null, waypointEventArgs);

        _roundRankingService.Verify(rrs => rrs.ConsumeCheckpointAsync(
            AccountId,
            1,
            1234,
            false,
            false
        ), Times.Once());
    }

    [Fact]
    public async Task Creates_Dnf_Entry_On_Player_Give_Up()
    {
        const string AccountId = "*fakeplayer1*";
        var playerGiveUpEventArgs = new PlayerUpdateEventArgs { Login = AccountId, AccountId = AccountId, Time = 0 };

        await Controller.OnPlayerGiveUpAsync(null, playerGiveUpEventArgs);

        _roundRankingService.Verify(rrs => rrs.ConsumeDnfAsync(AccountId), Times.Once());
    }

    [Fact]
    public async Task Clears_Checkpoints_Repository_On_Round_End()
    {
        await Controller.OnEndRoundAsync(null, EventArgs.Empty);

        _roundRankingService.Verify(rrs => rrs.ClearCheckpointDataAsync(), Times.Once());
    }

    [Fact]
    public async Task Clears_Checkpoints_Repository_On_Warmup_Round_End()
    {
        await Controller.OnWarmUpEndRoundAsync(null, new WarmUpRoundEventArgs { Current = 0, Total = 0 });

        _roundRankingService.Verify(rrs => rrs.ClearCheckpointDataAsync(), Times.Once());
    }

    [Fact]
    public async Task Clears_Checkpoints_Repository_On_Start_Match()
    {
        await Controller.OnStartMatchAsync(null, new MatchEventArgs());

        _roundRankingService.Verify(rrs => rrs.ClearCheckpointDataAsync(), Times.Once());
    }

    [Fact]
    public async Task Removes_Player_Checkpoint_Data_When_They_Start_Driving()
    {
        await Controller.OnStartLineAsync(null,
            new PlayerUpdateEventArgs { Login = "*fakeplayer1*", AccountId = "*fakeplayer1*", Time = 0 });

        _roundRankingService.Verify(rrs => rrs.RemovePlayerCheckpointDataAsync("*fakeplayer1*"), Times.Once());
    }

    [Fact]
    public async Task Removes_Checkpoint_Data_Of_Player_On_Respawn()
    {
        await Controller.OnRespawnAsync(null,
            new PlayerUpdateEventArgs { Login = "*fakeplayer1*", AccountId = "*fakeplayer1*", Time = 0 });

        _roundRankingService.Verify(rrs => rrs.RemovePlayerCheckpointDataAsync("*fakeplayer1*"), Times.Once());
    }

    [Fact]
    public async Task Hides_Widget_On_Podium_Start()
    {
        await Controller.OnPodiumStartAsync(null, EventArgs.Empty);

        _roundRankingService.Verify(rrs => rrs.HideRoundRankingWidgetAsync(), Times.Once());
    }

    [Fact]
    public async Task Sets_Up_Environment_On_Map_Start()
    {
        await Controller.OnStartMapAsync(null, EventArgs.Empty);

        _roundRankingService.Verify(rrs => rrs.DetectIsTeamsModeAsync(), Times.Once());
        _roundRankingService.Verify(rrs => rrs.FetchAndCacheTeamInfoAsync(), Times.Once());
        _roundRankingService.Verify(rrs => rrs.LoadPointsRepartitionFromSettingsAsync(), Times.Once());
        _roundRankingService.Verify(rrs => rrs.ClearCheckpointDataAsync(), Times.Once());
    }

    [Fact]
    public async Task Sets_Service_To_TimeAttack_Mode_During_Warm_Ups()
    {
        await Controller.OnWarmUpStartAsync(null, EventArgs.Empty);

        _roundRankingService.Verify(rrs => rrs.SetIsTimeAttackModeAsync(true), Times.Once());
    }

    [Fact]
    public async Task Disables_TimeAttack_Mode_After_Warm_Up()
    {
        await Controller.OnWarmUpEndAsync(null, EventArgs.Empty);

        _roundRankingService.Verify(rrs => rrs.SetIsTimeAttackModeAsync(false), Times.Once());
    }
}
