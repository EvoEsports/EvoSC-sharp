using System.Reflection;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Models.Players;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.FastestCp.Models;
using EvoSC.Modules.Official.FastestCp.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace EvoSC.Modules.Official.FastestCp.Tests;

public class FastestCpServiceTest
{
    private readonly FastestCpService _fastestCpService;
    private readonly ILogger<FastestCpService> _logger = new NullLogger<FastestCpService>();
    private readonly ILoggerFactory _loggerFactory = new NullLoggerFactory();
    private readonly Mock<IManialinkManager> _manialinkManagerMock = new();
    private readonly Mock<IPlayerManagerService> _playerManagerServiceMock = new();

    public FastestCpServiceTest()
    {
        _manialinkManagerMock
            .Setup(manager => manager.SendPersistentManialinkAsync(It.IsAny<string>(), It.IsAny<object>()))
            .Returns(Task.CompletedTask);
        _fastestCpService = new FastestCpService(_playerManagerServiceMock.Object, _manialinkManagerMock.Object,
            _loggerFactory, _logger);
    }

    [Fact]
    public async void Should_Register_Time_And_Update_Widget()
    {
        _playerManagerServiceMock.Setup(service => service.GetOrCreatePlayerAsync("AccountId1"))
            .ReturnsAsync(() => new Player
            {
                AccountId = "AccountId1", Id = 1, NickName = "NickName1", UbisoftName = "Ubisoft1"
            });

        await _fastestCpService.RegisterCpTimeAsync(new WayPointEventArgs
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
        });

        var actualData = new List<object>();
        _manialinkManagerMock.Verify(
            manager => manager.SendPersistentManialinkAsync("FastestCp.FastestCp", Capture.In(actualData)), Times.Once);
        Assert.NotEmpty(actualData);
        Assert.Equivalent(
            new { times = new List<PlayerCpTime?> { new("NickName1", 0, TimeSpan.FromMilliseconds(10)) } },
            actualData[0]);
    }

    [Fact]
    public async void Should_Register_Slower_Time_And_Not_Update_Widget()
    {
        Should_Register_Time_And_Update_Widget();
        _manialinkManagerMock.Invocations.Clear();

        await _fastestCpService.RegisterCpTimeAsync(new WayPointEventArgs
        {
            Login = "Login2",
            AccountId = "AccountId2",
            Speed = 0,
            BlockId = "",
            CheckpointInLap = 0,
            CheckpointInRace = 0,
            CurrentLapCheckpoints = Array.Empty<int>(),
            CurrentRaceCheckpoints = Array.Empty<int>(),
            Time = 20,
            LapTime = 20,
            RaceTime = 20,
            IsEndLap = false,
            IsEndRace = false
        });

        _manialinkManagerMock.Verify(
            manager => manager.SendPersistentManialinkAsync("FastestCp.FastestCp", It.IsAny<object>()), Times.Never);
    }

    [Fact]
    public async void Should_Reset_Store()
    {
        var fieldInfo = _fastestCpService.GetType()
            .GetField("_fastestCpStore", BindingFlags.NonPublic | BindingFlags.Instance);
        var before = fieldInfo?.GetValue(_fastestCpService);

        await _fastestCpService.ResetCpTimesAsync();

        var after = fieldInfo?.GetValue(_fastestCpService);


        _manialinkManagerMock.Verify(manager => manager.HideManialinkAsync("FastestCp.FastestCp"), Times.Once);

        Assert.NotNull(before);
        Assert.NotNull(after);
        Assert.NotSame(before, after);
    }
}
