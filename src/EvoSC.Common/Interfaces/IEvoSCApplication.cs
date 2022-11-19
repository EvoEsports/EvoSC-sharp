using SimpleInjector;

namespace EvoSC.Common.Interfaces;

public interface IEvoSCApplication
{
    /// <summary>
    /// Global cancellation token of the application. This is set when ShutdownAsync is called.
    /// </summary>
    public CancellationToken MainCancellationToken { get; }
    
    /// <summary>
    /// The core service container that the application uses.
    /// </summary>
    public Container Services { get; }
    
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
