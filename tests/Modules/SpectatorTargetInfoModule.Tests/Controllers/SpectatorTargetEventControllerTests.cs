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

        await Controller.OnPlayerDisconnectAsync(null, new PlayerGbxEventArgs { Login = login });

        _spectatorTargetService.Verify(sts => sts.RemovePlayerFromSpectatorsListAsync(login));
    }

    [Fact]
    public async Task Updates_Team_Mode_On_New_Map()
    {
        await Controller.OnBeginMapAsync(null, new MapGbxEventArgs());

        _spectatorTargetService.Verify(sts => sts.UpdateIsTeamsModeAsync());
    }

    [Fact]
    public async Task Registers_Ceckpoint_Times()
    {
        var login = "*fakeplayer_unittest*";
        var checkpointId = 3;
        var lapTime = 1234;

        await Controller.OnWayPointAsync(null,
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
                CurrentRaceCheckpoints = null,
                CurrentLapCheckpoints = null,
                BlockId = null,
                Speed = 0,
                Time = 0
            });

        _spectatorTargetService.Verify(sts => sts.AddCheckpointAsync(login, checkpointId, lapTime));
    }

    [Fact]
    public async Task Resets_Collected_Data_On_New_Round()
    {
        await Controller.OnNewRoundAsync(null, new RoundEventArgs { Count = 0, Time = 0 });

        _spectatorTargetService.Verify(sts => sts.ClearCheckpointsAsync());
        _spectatorTargetService.Verify(sts => sts.FetchAndCacheTeamInfoAsync());
    }
}
