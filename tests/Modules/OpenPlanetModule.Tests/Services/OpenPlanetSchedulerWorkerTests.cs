using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.OpenPlanetModule;
using EvoSC.Modules.Official.OpenPlanetModule.Config;
using EvoSC.Modules.Official.OpenPlanetModule.Interfaces;
using EvoSC.Modules.Official.OpenPlanetModule.Models;
using EvoSC.Modules.Official.OpenPlanetModule.Services;
using Moq;

namespace EvoSC.Modules.Official.OpenPlanetModule.Tests.Services;

public class OpenPlanetSchedulerWorkerTests
{
    private readonly Mock<IEventManager> _events = new();
    private readonly Mock<IPlayer> _player = new();
    private readonly Mock<IOpenPlanetControlSettings> _settings = new();
    
    [Fact]
    public async Task Player_Due_Is_Triggered_From_Loop()
    {
        _settings.Setup(m => m.KickTimeout).Returns(0);
        IOpenPlanetScheduler scheduler = new OpenPlanetScheduler(_settings.Object, _events.Object);
        
        scheduler.ScheduleKickPlayer(_player.Object);

        var schedulerWorker = new OpenPlanetSchedulerWorker(scheduler);

        await schedulerWorker.StartAsync();
        await Task.Delay(2000);
        
        _events.Verify(m => m.RaiseAsync(OpenPlanetEvents.PlayerDueForKick, It.IsAny<PlayerDueForKickEventArgs>()),
            Times.Once);
    }
    
    [Fact]
    public async Task Worker_Is_Stopped()
    {
        var scheduler = new Mock<IOpenPlanetScheduler>();

        var schedulerWorker = new OpenPlanetSchedulerWorker(scheduler.Object);

        await schedulerWorker.StartAsync();
        await schedulerWorker.StopAsync();
        await Task.Delay(2000);

       scheduler.Verify(m => m.TriggerDuePlayerKicks(), Times.Never);
    }
}
