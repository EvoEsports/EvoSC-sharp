using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.OpenPlanetModule;
using EvoSC.Modules.Official.OpenPlanetModule.Config;
using EvoSC.Modules.Official.OpenPlanetModule.Interfaces;
using EvoSC.Modules.Official.OpenPlanetModule.Models;
using EvoSC.Modules.Official.OpenPlanetModule.Services;
using Moq;

namespace EvoSC.Modules.Official.OpenPlanetModule.Tests.Services;

public class OpenPlanetSchedulerTests
{
    private readonly Mock<IEventManager> _events = new();
    private readonly Mock<IPlayer> _player = new();
    private readonly Mock<IOpenPlanetControlSettings> _settings = new();

    [Fact]
    public void Player_Is_Scheduled()
    {
        IOpenPlanetScheduler scheduler = new OpenPlanetScheduler(_settings.Object, _events.Object);
        
        scheduler.ScheduleKickPlayer(_player.Object);

        var isScheduled = scheduler.PlayerIsScheduledForKick(_player.Object);
        
        Assert.True(isScheduled);
    }

    [Fact]
    public void Player_Is_Unscheduled()
    {
        IOpenPlanetScheduler scheduler = new OpenPlanetScheduler(_settings.Object, _events.Object);
        
        scheduler.ScheduleKickPlayer(_player.Object);
        scheduler.UnScheduleKickPlayer(_player.Object);

        var isScheduled = scheduler.PlayerIsScheduledForKick(_player.Object);
        
        Assert.False(isScheduled);
    }

    [Fact]
    public async Task Player_Scheduled_Is_Triggered_When_Due()
    {
        _settings.Setup(m => m.KickTimeout).Returns(0);
        IOpenPlanetScheduler scheduler = new OpenPlanetScheduler(_settings.Object, _events.Object);

        scheduler.ScheduleKickPlayer(_player.Object);

        await Task.Delay(1000);
        await scheduler.TriggerDuePlayerKicks();

        _events.Verify(m => m.RaiseAsync(OpenPlanetEvents.PlayerDueForKick, It.IsAny<PlayerDueForKickEventArgs>()),
            Times.Once);
    }

    [Fact]
    public async Task Scheduled_Players_Not_Due_Are_Not_Triggered()
    {
        _settings.Setup(m => m.KickTimeout).Returns(5);
        IOpenPlanetScheduler scheduler = new OpenPlanetScheduler(_settings.Object, _events.Object);

        scheduler.ScheduleKickPlayer(_player.Object);

        await scheduler.TriggerDuePlayerKicks();

        _events.Verify(m => m.RaiseAsync(OpenPlanetEvents.PlayerDueForKick, It.IsAny<PlayerDueForKickEventArgs>()),
            Times.Never);
    }
}
