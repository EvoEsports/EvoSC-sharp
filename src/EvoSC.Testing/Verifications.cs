using Microsoft.Extensions.Logging;
using Moq;

namespace EvoSC.Testing;

public static class Verifications
{
    /// <summary>
    /// Helper method for verifying that a logging call was made.
    /// </summary>
    /// <param name="loggerMock"></param>
    /// <param name="logLevel">The log level of the log.</param>
    /// <param name="exception">Exception if present that was logged.</param>
    /// <param name="msg">Message which was logged.</param>
    /// <param name="times">How many times this log was called.</param>
    /// <typeparam name="T">The type which this logger is assigned to.</typeparam>
    public static void Verify<T>(this Mock<ILogger<T>> loggerMock, LogLevel logLevel, Exception? exception, string? msg,
        Times times)
    {
        if (exception == null)
        {
            loggerMock.Verify(m => m.Log(
                logLevel,
                0,
                It.Is<It.IsAnyType>((o, type) =>
                    msg == null || (
                        o.ToString()!.StartsWith(msg, StringComparison.Ordinal) 
                        &&type.Name.Equals("FormattedLogValues", StringComparison.Ordinal)
                    )),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()
            ), times);
        }
        else
        {
            // duplicate code is required due to the way moq works with it's expression system
            loggerMock.Verify(m => m.Log(
                logLevel,
                0,
                It.Is<It.IsAnyType>((o, type) =>
                    msg == null || (
                        o.ToString()!.StartsWith(msg, StringComparison.Ordinal) 
                        &&type.Name.Equals("FormattedLogValues", StringComparison.Ordinal)
                    )),
                exception,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()
            ), times);
        }
    }
}
