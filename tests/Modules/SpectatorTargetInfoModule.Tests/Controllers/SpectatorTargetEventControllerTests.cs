using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.SpectatorTargetInfoModule.Controllers;
using EvoSC.Modules.Official.SpectatorTargetInfoModule.Interfaces;
using EvoSC.Testing.Controllers;
using GbxRemoteNet.Events;
using Moq;
using Xunit;

namespace EvoSC.Modules.Official.SpectatorTargetInfoModule.Tests.Controllers;

public class
    SpectatorTargetEventControllerTests : ControllerMock<SpectatorTargetInfoEventController, IEventControllerContext>
{
    private Mock<ISpectatorTargetInfoService> _spectatorTargetService = new();

    public SpectatorTargetEventControllerTests()
    {
        InitMock(_spectatorTargetService);
    }

    [Fact]
    public async Task Removes_Player_From_Spectators_On_Disconnect()
    {
        var login = "*fakeplayer_unittest*";

        await Controller.OnPlayerDisconnectAsync(null!, new PlayerGbxEventArgs { Login = login });

        _spectatorTargetService.Verify(sts => sts.RemovePlayerAsync(login));
    }

    [Fact]
    public async Task Detects_Team_And_TimeAttack_Mode_On_New_Map()
    {
        await Controller.OnBeginMapAsync(null!, new MapGbxEventArgs());

        _spectatorTargetService.Verify(sts => sts.DetectIsTeamsModeAsync());
        _spectatorTargetService.Verify(sts => sts.DetectIsTimeAttackModeAsync());
    }

    [Fact]
    public async Task Registers_Checkpoint_Times()
    {
        var login = "*fakeplayer_unittest*";
        var checkpointId = 3;
        var lapTime = 1234;

        await Controller.OnWayPointAsync(null!,
            new WayPointEventArgs
            {
                Login = login,
                AccountId = login,
                RaceTime = 0,
                LapTime = lapTime,
                CheckpointInRace = 0,
                CheckpointInLap = checkpointId,
                IsEndRace = false,
                IsEndLap = false,
                CurrentRaceCheckpoints = [],
                CurrentLapCheckpoints = [],
                BlockId = "",
                Speed = 0,
                Time = 0
            });

        _spectatorTargetService.Verify(sts => sts.AddCheckpointAsync(login, checkpointId, lapTime));
    }

    [Fact]
    public async Task Resets_Collected_Data_On_New_Round()
    {
        await Controller.OnNewRoundAsync(null!, new RoundEventArgs { Count = 0, Time = 0 });

        _spectatorTargetService.Verify(sts => sts.ClearCheckpointsAsync());
        _spectatorTargetService.Verify(sts => sts.FetchAndCacheTeamInfoAsync());
        _spectatorTargetService.Verify(sts => sts.ResetWidgetForSpectatorsAsync());
    }

    [Fact]
    public async Task Resets_Collected_Data_On_New_Warm_Up_Round()
    {
        await Controller.OnNewWarmUpRoundAsync(null!, new WarmUpRoundEventArgs { Total = 3, Current = 1 });

        _spectatorTargetService.Verify(sts => sts.ClearCheckpointsAsync());
        _spectatorTargetService.Verify(sts => sts.FetchAndCacheTeamInfoAsync());
        _spectatorTargetService.Verify(sts => sts.ResetWidgetForSpectatorsAsync());
    }

    [Fact]
    public async Task Sets_TimeAttack_Mode_To_Active_At_Warm_Up_Start()
    {
        await Controller.OnWarmUpStartAsync(null!, EventArgs.Empty);

        _spectatorTargetService.Verify(sts => sts.UpdateIsTimeAttackModeAsync(true));
    }

    [Fact]
    public async Task Detects_TimeAttack_Mode_At_Warm_Up_End()
    {
        await Controller.OnWarmUpEndAsync(null!, EventArgs.Empty);

        _spectatorTargetService.Verify(sts => sts.DetectIsTimeAttackModeAsync());
    }

    [Fact]
    public async Task Clears_Checkpoints_Of_Player_On_Give_Up()
    {
        var eventArgs = new PlayerUpdateEventArgs { Login = "*fakeplayer1*", AccountId = "*fakeplayer1*", Time = 0 };

        await _spectatorTargetService.Object.UpdateIsTimeAttackModeAsync(true);
        await Controller.OnPlayerGiveUpAsync(null!, eventArgs);

        _spectatorTargetService.Verify(sts => sts.ClearCheckpointsAsync(eventArgs.Login));
    }
}
