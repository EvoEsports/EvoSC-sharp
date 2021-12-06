using System.Threading;
using EvoSC.Domain;
using GameHost.V3;
using NLog;
using NLog.Config;
using NLog.Targets;
using EvoSC.Injection;
using EvoSC.Modules.CLI;

using var runner = GameHostInit.Launch(
    sc =>
    {
        
        AddLogger(sc);
        
    },
    //sc => new EvoSCEntryModule(sc)
    sc => new Module(sc)
);

while (runner.Loop())
{
    // update 40 times per second
    Thread.Sleep(25);
}

LogManager.Shutdown();

void AddLogger(Scope scope)
{
    LoggingConfiguration config = new();

    FileTarget logFile = new("logfile") {FileName = "Logs/log.txt"};
    ConsoleTarget logConsole = new("logconsole");

    config.AddRule(LogLevel.Info, LogLevel.Fatal, logConsole);
    config.AddRule(LogLevel.Debug, LogLevel.Fatal, logFile);

    LogManager.Configuration = config;

    scope.Context.Register(typeof(ILogger), new TransientLogger());
}
