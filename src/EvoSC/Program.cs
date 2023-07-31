
using EvoSC;
using EvoSC.CLI;

return await new CliManager()
    .RegisterCommands(typeof(Application).Assembly)
    .ExecuteAsync(args);
