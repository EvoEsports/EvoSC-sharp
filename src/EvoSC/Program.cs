
using System.CommandLine;
using EvoSC;
using EvoSC.CLI;
using EvoSC.CliCommands;

return await new CliHandler(args)
    .RegisterCommand(new RunCommand())
    .Handle();
