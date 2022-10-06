namespace EvoSC.Common.Interfaces;

public interface IEvoSCApplication
{
    public CancellationToken MainCancellationToken { get; }
    
    public Task RunAsync();
    public Task ShutdownAsync();
}
