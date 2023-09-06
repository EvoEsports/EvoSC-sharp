using EvoSC.Common.Interfaces;
using SimpleInjector;

namespace EvoSC.CLI;

public class EvoScCliApp : IEvoSCApplication
{
    public IStartupPipeline StartupPipeline { get; }
    public CancellationToken MainCancellationToken { get; }
    public Container Services { get; }

    public EvoScCliApp()
    {
        
    }
    
    public Task RunAsync()
    {
        throw new NotImplementedException();
    }

    public Task ShutdownAsync()
    {
        throw new NotImplementedException();
    }
}
