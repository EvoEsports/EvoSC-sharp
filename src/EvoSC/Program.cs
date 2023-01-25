
using EvoSC.CLI;
using EvoSC.CliCommands;

return await new CliHandler(args)
    .RegisterCommand(new RunCommand())
    .HandleAsync();
