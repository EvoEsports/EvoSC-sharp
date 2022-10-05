
using System.CommandLine;
using EvoSC;
using EvoSC.CLI;

return await new CliHandler(args)
    .RegisterCommand(new RunCommand())
    .Handle();
