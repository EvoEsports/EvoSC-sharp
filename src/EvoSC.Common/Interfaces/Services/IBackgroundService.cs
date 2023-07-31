namespace EvoSC.Common.Interfaces.Services;

public interface IBackgroundService
{
    public Task StartAsync();
    public Task StopAsync();
}
