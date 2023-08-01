using System.Timers;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Services.Attributes;
using EvoSC.Modules.Official.OpenPlanetModule.Interfaces;
using Timer = System.Timers.Timer;

namespace EvoSC.Modules.Official.OpenPlanetModule.Services;

[Service]
public class OpenPlanetSchedulerWorker : IBackgroundService
{
    private readonly IOpenPlanetScheduler _scheduler;
    private readonly Timer _schedulerLoop = new(TimeSpan.FromSeconds(1));
    
    public OpenPlanetSchedulerWorker(IOpenPlanetScheduler scheduler)
    {
        _scheduler = scheduler;
        
        _schedulerLoop.Elapsed += SchedulerLoopOnElapsed;
        _schedulerLoop.AutoReset = true;
    }

    private void SchedulerLoopOnElapsed(object? sender, ElapsedEventArgs e)
    {
        _scheduler.TriggerDuePlayerKicks().GetAwaiter().GetResult();
    }

    public Task StartAsync()
    {
        _schedulerLoop.Start();
        return Task.CompletedTask;
    }

    public Task StopAsync()
    {
        _schedulerLoop.Start();
        return Task.CompletedTask;
    }
}
