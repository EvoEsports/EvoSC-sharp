using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.OpenPlanetModule.Config;
using EvoSC.Modules.Official.OpenPlanetModule.Interfaces;
using EvoSC.Modules.Official.OpenPlanetModule.Models;
using EvoSC.Modules.Official.OpenPlanetModule.Services;
using NSubstitute;

namespace EvoSC.Modules.Official.OpenPlanetModule.Tests.Services;

public class OpenPlanetSchedulerTests
{
    private readonly IEventManager _events = Substitute.For<IEventManager>();
    private readonly IPlayer _player = Substitute.For<IPlayer>();
    private readonly IOpenPlanetControlSettings _settings = Substitute.For<IOpenPlanetControlSettings>();

    public OpenPlanetSchedulerTests()
    {
        _player.AccountId.Returns("something");
    }

    [Fact]
    public void Player_Is_Scheduled()
    {
        IOpenPlanetScheduler scheduler = new OpenPlanetScheduler(_settings, _events);
        
        scheduler.ScheduleKickPlayer(_player);

        var isScheduled = scheduler.PlayerIsScheduledForKick(_player);
        
        Assert.True(isScheduled);
    }

    [Fact]
    public void Player_Is_Unscheduled()
    {
        IOpenPlanetScheduler scheduler = new OpenPlanetScheduler(_settings, _events);
        
        scheduler.ScheduleKickPlayer(_player);
        scheduler.UnScheduleKickPlayer(_player);

        var isScheduled = scheduler.PlayerIsScheduledForKick(_player);
        
        Assert.False(isScheduled);
    }

    [Fact]
    public async Task Player_Scheduled_Is_Triggered_When_Due()
    {
        _settings.KickTimeout.Returns(0);
        IOpenPlanetScheduler scheduler = new OpenPlanetScheduler(_settings, _events);

        scheduler.ScheduleKickPlayer(_player);

        await Task.Delay(1000);
        await scheduler.TriggerDuePlayerKicksAsync();

        await _events.Received(1).RaiseAsync(OpenPlanetEvents.PlayerDueForKick, Arg.Any<PlayerDueForKickEventArgs>());
    }

    [Fact]
    public async Task Scheduled_Players_Not_Due_Are_Not_Triggered()
    {
        _settings.KickTimeout.Returns(5);
        IOpenPlanetScheduler scheduler = new OpenPlanetScheduler(_settings, _events);

        scheduler.ScheduleKickPlayer(_player);

        await scheduler.TriggerDuePlayerKicksAsync();

        await _events.DidNotReceive().RaiseAsync(OpenPlanetEvents.PlayerDueForKick, Arg.Any<PlayerDueForKickEventArgs>());
        
    }
}
