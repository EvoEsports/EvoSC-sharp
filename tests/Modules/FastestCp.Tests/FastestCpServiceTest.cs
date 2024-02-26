using System.Reflection;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Models.Players;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.FastestCpModule.Models;
using EvoSC.Modules.Official.FastestCpModule.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;

namespace EvoSC.Modules.Official.FastestCpModule.Tests;

public class FastestCpServiceTest
{
    private readonly FastestCpService _fastestCpService;
    private readonly ILogger<FastestCpService> _logger = new NullLogger<FastestCpService>();
    private readonly ILoggerFactory _loggerFactory = new NullLoggerFactory();
    private readonly IManialinkManager _manialinkManagerMock = Substitute.For<IManialinkManager>();
    private readonly IPlayerManagerService _playerManagerServiceMock = Substitute.For<IPlayerManagerService>();

    public FastestCpServiceTest()
    {
        _manialinkManagerMock
            .SendPersistentManialinkAsync(Arg.Any<string>(), Arg.Any<object>())
            .Returns(Task.CompletedTask);
        _fastestCpService = new FastestCpService(_playerManagerServiceMock, _manialinkManagerMock,
            _loggerFactory, _logger);
    }

    [Fact]
    public async Task Should_Register_Time_And_Update_Widget()
    {
        _playerManagerServiceMock.GetOrCreatePlayerAsync("AccountId1")
            .Returns(new Player
            {
                AccountId = "AccountId1", Id = 1, NickName = "NickName1", UbisoftName = "Ubisoft1"
            });
        object? actualData = null;
        await _manialinkManagerMock.SendPersistentManialinkAsync(Arg.Any<string>(), Arg.Do<object>(x => actualData = x));

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
        
        await _manialinkManagerMock.Received(1).SendPersistentManialinkAsync("FastestCpModule.FastestCp", Arg.Any<object>());
        Assert.Equivalent(
            new { times = new List<PlayerCpTime?> { new("NickName1", 0, TimeSpan.FromMilliseconds(10)) } },
            actualData);
    }

    [Fact]
    public async Task Should_Register_Slower_Time_And_Not_Update_Widget()
    {
        Should_Register_Time_And_Update_Widget();
        _manialinkManagerMock.ClearReceivedCalls();

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

        await _manialinkManagerMock.DidNotReceive().SendPersistentManialinkAsync("FastestCpModule.FastestCp", Arg.Any<object>());
    }

    [Fact]
    public async Task Should_Reset_Store()
    {
        var fieldInfo = _fastestCpService.GetType()
            .GetField("_fastestCpStore", BindingFlags.NonPublic | BindingFlags.Instance);
        var before = fieldInfo?.GetValue(_fastestCpService);

        await _fastestCpService.ResetCpTimesAsync();

        var after = fieldInfo?.GetValue(_fastestCpService);


        await _manialinkManagerMock.Received(1).HideManialinkAsync("FastestCpModule.FastestCp");

        Assert.NotNull(before);
        Assert.NotNull(after);
        Assert.NotSame(before, after);
    }
}
