using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.FastestCpModule.Controllers;
using EvoSC.Modules.Official.FastestCpModule.Interfaces;
using GbxRemoteNet.Events;
using NSubstitute;

namespace EvoSC.Modules.Official.FastestCpModule.Tests;

public class FastestCpControllerTest
{
    private readonly FastestCpController _fastestCpController;
    private readonly IFastestCpService _fastestCpService = Substitute.For<IFastestCpService>();

    public FastestCpControllerTest()
    {
        _fastestCpController = new FastestCpController(_fastestCpService);
    }

    [Fact]
    public async Task Should_Register_Time_And_Update_Widget()
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
        await _fastestCpService.Received(1).RegisterCpTimeAsync(waypoint);
    }

    [Fact]
    public async Task Should_Reset_Store()
    {
        await _fastestCpController.ResetCpTimesAsync(new object(), new MapGbxEventArgs { Map = null });
        await _fastestCpService.Received(1).ResetCpTimesAsync();
    }
}
