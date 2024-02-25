using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.OpenPlanetModule.Config;
using EvoSC.Modules.Official.OpenPlanetModule.Interfaces;
using EvoSC.Modules.Official.OpenPlanetModule.Models;
using EvoSC.Modules.Official.OpenPlanetModule.Services;
using NSubstitute;

namespace EvoSC.Modules.Official.OpenPlanetModule.Tests.Services;

public class OpenPlanetSchedulerWorkerTests
{
    private readonly IEventManager _events = Substitute.For<IEventManager>();
    private readonly IPlayer _player = Substitute.For<IPlayer>();
    private readonly IOpenPlanetControlSettings _settings = Substitute.For<IOpenPlanetControlSettings>();

    public OpenPlanetSchedulerWorkerTests()
    {
        _player.AccountId.Returns("something");
    }
    
    [Fact]
    public async Task Player_Due_Is_Triggered_From_Loop()
    {
        _settings.KickTimeout.Returns(0);
        IOpenPlanetScheduler scheduler = new OpenPlanetScheduler(_settings, _events);
        
        scheduler.ScheduleKickPlayer(_player);

        var schedulerWorker = new OpenPlanetSchedulerWorker(scheduler);

        await schedulerWorker.StartAsync();
        await Task.Delay(2000);
        
        await _events.Received(1).RaiseAsync(OpenPlanetEvents.PlayerDueForKick, Arg.Any<PlayerDueForKickEventArgs>());
    }
    
    [Fact]
    public async Task Worker_Is_Stopped()
    {
        var scheduler = Substitute.For<IOpenPlanetScheduler>();

        var schedulerWorker = new OpenPlanetSchedulerWorker(scheduler);

        await schedulerWorker.StartAsync();
        await schedulerWorker.StopAsync();
        await Task.Delay(2000);

       await scheduler.DidNotReceive().TriggerDuePlayerKicksAsync();
    }
}
