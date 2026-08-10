using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;

namespace EvoSC.Common.Logging;

/// <summary>
/// Console formatter that prints the log level as an uppercase word, e.g. "DEBUG some message".
/// </summary>
public class EvoScConsoleFormatter : ConsoleFormatter
{
    public const string FormatterName = "evosc";

    private const string AnsiReset = "\x1B[0m";

    public EvoScConsoleFormatter() : base(FormatterName)
    {
    }

    public override void Write<TState>(in LogEntry<TState> logEntry, IExternalScopeProvider? scopeProvider,
        TextWriter textWriter)
    {
        var message = logEntry.Formatter(logEntry.State, logEntry.Exception);

        if (string.IsNullOrEmpty(message) && logEntry.Exception is null)
        {
            return;
        }

        var timestamp = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff");

        textWriter.Write('[');
        textWriter.Write(timestamp);
        textWriter.Write("] ");

        textWriter.Write(GetLevelColor(logEntry.LogLevel));
        textWriter.Write(GetLevelText(logEntry.LogLevel));
        textWriter.Write(AnsiReset);

        textWriter.Write(' ');
        textWriter.Write(logEntry.Category);
        textWriter.Write(": ");
        textWriter.Write(message);

        if (logEntry.Exception is not null)
        {
            textWriter.Write(' ');
            textWriter.Write(logEntry.Exception);
        }

        textWriter.Write(Environment.NewLine);
    }

    private static string GetLevelText(LogLevel level) => level switch
    {
        LogLevel.Trace => "TRACE",
        LogLevel.Debug => "DEBUG",
        LogLevel.Information => "INFO",
        LogLevel.Warning => "WARN",
        LogLevel.Error => "ERROR",
        LogLevel.Critical => "CRIT",
        _ => "NONE"
    };

    private static string GetLevelColor(LogLevel level) => level switch
    {
        LogLevel.Trace => "\x1B[90m",
        LogLevel.Debug => "\x1B[90m",
        LogLevel.Information => "\x1B[32m",
        LogLevel.Warning => "\x1B[33m",
        LogLevel.Error => "\x1B[31m",
        LogLevel.Critical => "\x1B[1m\x1B[31m",
        _ => AnsiReset
    };
}
