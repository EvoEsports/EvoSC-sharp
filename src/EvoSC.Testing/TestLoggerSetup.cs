using Microsoft.Extensions.Logging;

namespace EvoSC.Testing;

public static class TestLoggerSetup
{
    /// <summary>
    /// Create a new logger.
    /// </summary>
    /// <typeparam name="T">The class to bind the logger to.</typeparam>
    /// <returns></returns>
    public static ILogger<T> CreateLogger<T>() =>
        LoggerFactory.Create(c => { }).CreateLogger<T>();
}
