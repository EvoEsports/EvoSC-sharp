using Microsoft.Extensions.Logging;
using Moq;

namespace EvoSC.Testing;

public static class Verifications
{
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
        

        /* loggerMock.Verify(m => m.Log(
            LogLevel.Trace,
            0,
            It.IsAny<It.IsAnyType>(),
            ex,
            It.IsAny<Func<It.IsAnyType, Exception, string>>()
        ), Times.Once); */
    }
}
