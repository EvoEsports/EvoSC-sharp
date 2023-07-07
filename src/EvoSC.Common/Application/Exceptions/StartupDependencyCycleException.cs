namespace EvoSC.Common.Application.Exceptions;

/// <summary>
/// Exception that occurs when the startup pipeline detects a dependency cycle.
/// </summary>
public class StartupDependencyCycleException : StartupPipelineException
{
    /// <summary>
    /// </summary>
    /// <param name="dependencyPath">The path which the cycle occurs in.</param>
    public StartupDependencyCycleException(IEnumerable<string> dependencyPath) : 
        base($"Startup dependency cycle detected: {string.Join(" -> ", dependencyPath)}")
    {
    }
}
