using Microsoft.Extensions.Logging;

namespace EvoSC.Common.Tests;

public static class LoggerSetup
{
    /// <summary>
    /// Create a new logger.
    /// </summary>
    /// <typeparam name="T">The class to bind the logger to.</typeparam>
    /// <returns></returns>
    public static ILogger<T> CreateLogger<T>() =>
        LoggerFactory.Create(c => { }).CreateLogger<T>();
}
