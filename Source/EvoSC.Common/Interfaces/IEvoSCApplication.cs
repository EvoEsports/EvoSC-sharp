namespace EvoSC.Common.Interfaces;

public interface IEvoSCApplication
{
    public Task RunAsync();
    public Task ShutdownAsync();
}
