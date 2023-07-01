namespace EvoSC.Common.Application.Exceptions;

public class StartupDependencyCycleException : StartupPipelineException
{
    public StartupDependencyCycleException(IEnumerable<string> dependencyPath) : 
        base($"Startup dependency cycle detected: {string.Join(" -> ", dependencyPath)}")
    {
    }
}
