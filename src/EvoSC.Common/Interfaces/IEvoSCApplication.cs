namespace EvoSC.Common.Interfaces;

public interface IEvoSCApplication
{
    public CancellationToken MainCancellationToken { get; }
    
    /// <summary>
    /// Initialize and start EvoSC#.
    /// </summary>
    /// <returns></returns>
    public Task RunAsync();
    /// <summary>
    /// Graceful shutdown of EvoSC#.
    /// </summary>
    /// <returns></returns>
    public Task ShutdownAsync();
}
