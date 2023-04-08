using System.Reflection;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Models.Players;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.FastestCp.Controllers;
using EvoSC.Modules.Official.FastestCp.Interfaces;
using EvoSC.Modules.Official.FastestCp.Models;
using EvoSC.Modules.Official.FastestCp.Services;
using GbxRemoteNet.Events;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace EvoSC.Modules.Official.FastestCp.Tests;

public class FastestCpControllerTest
{
    private readonly Mock<IFastestCpService> _fastestCpService = new();

    private readonly FastestCpController _fastestCpController;

    public FastestCpControllerTest()
    {
        _fastestCpController = new FastestCpController(_fastestCpService.Object);
    }

    [Fact]
    public async void ShouldRegisterTimeAndShowWidget()
    {
        var waypoint = new WayPointEventArgs
        {
            Login = "Login1",
            AccountId = "AccountId1",
            Speed = 0,
            BlockId = "",
            CheckpointInLap = 0,
            CheckpointInRace = 0,
            CurrentLapCheckpoints = new int[0],
            CurrentRaceCheckpoints = new int[0],
            Time = 10,
            LapTime = 10,
            RaceTime = 10,
            IsEndLap = false,
            IsEndRace = false
        };

        await _fastestCpController.RegisterCpTime(new object(), waypoint);
        _fastestCpService.Verify(service => service.RegisterCpTime(waypoint), Times.Once);
    }

    [Fact]
    public async void ShouldResetStore()
    {
        await _fastestCpController.ResetCpTimes(new object(), new MapGbxEventArgs { Map = null });
        _fastestCpService.Verify(service => service.ResetCpTimes(), Times.Once);
    }
}
