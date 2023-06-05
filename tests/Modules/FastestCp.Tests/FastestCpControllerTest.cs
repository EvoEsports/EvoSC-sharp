using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.FastestCp.Controllers;
using EvoSC.Modules.Official.FastestCp.Interfaces;
using GbxRemoteNet.Events;
using Moq;

namespace EvoSC.Modules.Official.FastestCp.Tests;

public class FastestCpControllerTest
{
    private readonly FastestCpController _fastestCpController;
    private readonly Mock<IFastestCpService> _fastestCpService = new();

    public FastestCpControllerTest()
    {
        _fastestCpController = new FastestCpController(_fastestCpService.Object);
    }

    [Fact]
    public async void Should_Register_Time_And_Update_Widget()
    {
        var waypoint = new WayPointEventArgs
        {
            Login = "Login1",
            AccountId = "AccountId1",
            Speed = 0,
            BlockId = "",
            CheckpointInLap = 0,
            CheckpointInRace = 0,
            CurrentLapCheckpoints = Array.Empty<int>(),
            CurrentRaceCheckpoints = Array.Empty<int>(),
            Time = 10,
            LapTime = 10,
            RaceTime = 10,
            IsEndLap = false,
            IsEndRace = false
        };

        await _fastestCpController.RegisterCpTimeAsync(new object(), waypoint);
        _fastestCpService.Verify(service => service.RegisterCpTimeAsync(waypoint), Times.Once);
    }

    [Fact]
    public async void Should_Reset_Store()
    {
        await _fastestCpController.ResetCpTimesAsync(new object(), new MapGbxEventArgs { Map = null });
        _fastestCpService.Verify(service => service.ResetCpTimesAsync(), Times.Once);
    }
}
