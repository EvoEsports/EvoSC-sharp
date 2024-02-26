using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReceivedExtensions;

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
    public static void Verify<T>(this ILogger<T> loggerMock, LogLevel logLevel, Exception? exception, string? msg, Quantity times)
    {
        if (exception == null)
        {
            loggerMock.Received(times).Log(
                logLevel,
                Arg.Any<EventId>(),
                Arg.Is<object>(o =>  msg == null || (
                    o.ToString()!.StartsWith(msg, StringComparison.Ordinal) 
                    && o.GetType().Name.Equals("FormattedLogValues", StringComparison.Ordinal)
                )),
                Arg.Any<Exception>(),
                Arg.Any<Func<object, Exception?, string>>());
        }
        else
        {
            loggerMock.Received(times).Log(
                logLevel,
                Arg.Any<EventId>(),
                Arg.Is<object>(o =>  msg == null || (
                    o.ToString()!.StartsWith(msg, StringComparison.Ordinal) 
                    && o.GetType().Name.Equals("FormattedLogValues", StringComparison.Ordinal)
                )),
                exception,
                Arg.Any<Func<object, Exception?, string>>());
        }
    }
}
