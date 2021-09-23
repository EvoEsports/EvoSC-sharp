using System.Threading;
using GameHost.V3;
using NLog;
using NLog.Config;
using NLog.Targets;
using EvoSC.Injection;

using var runner = GameHostInit.Launch(
    sc =>
    {
        addLogger(sc);
    },
    //sc => new EvoSCEntryModule(sc)
    sc => new EvoSC.CLI.Module(sc)
);

while (runner.Loop())
{
    // update 40 times per second
    Thread.Sleep(25);
}

LogManager.Shutdown();

void addLogger(Scope scope)
{
    LoggingConfiguration config = new();

    FileTarget logFile = new("logfile") {FileName = "Logs/log.txt"};
    ConsoleTarget logConsole = new("logconsole");

    config.AddRule(LogLevel.Info, LogLevel.Fatal, logConsole);
    config.AddRule(LogLevel.Debug, LogLevel.Fatal, logFile);

    LogManager.Configuration = config;

    scope.Context.Register(typeof(ILogger), new TransientLogger());
}
