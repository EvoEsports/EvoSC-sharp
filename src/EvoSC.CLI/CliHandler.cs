using System.CommandLine;
using System.Reflection;
using EvoSC.CLI.Attributes;
using EvoSC.CLI.Exceptions;
using EvoSC.CLI.Interfaces;

namespace EvoSC.CLI;

public class CliHandler
{
    private RootCommand _rootCommand;
    private readonly string[] _args;
    
    public CliHandler(string[] args)
    {
        _args = args;
        _rootCommand = new RootCommand("EvoSC TrackMania Server Controller.");
    }

    public CliHandler RegisterCommand(ICliCommand cliCmd)
    {
        var cmdAttr = GetCommandInfo(cliCmd);
        var cmd = new Command(cmdAttr.Name, cmdAttr.Description);

        var options = GetCommandOptions(cliCmd);
        foreach (var option in options)
        {
            cmd.AddOption(option);
        }
        
        cmd.SetHandler((invocationContext) =>
        {
            var context = new CliCommandContext(invocationContext, _args);
            cliCmd.ExecuteAsync(CancellationToken.None, context).GetAwaiter().GetResult();
        });
        
        _rootCommand.AddCommand(cmd);
        return this;
    }

    public Task<int> Handle()
    {
        return _rootCommand.InvokeAsync(_args);
    }
    
    private IEnumerable<Option> GetCommandOptions(ICliCommand cliCmd)
    {
        var cmdOptions = cliCmd.GetType().GetCustomAttributes<CliOptionAttribute>();
        var options = new List<Option>();

        foreach (var cmdOption in cmdOptions)
        {
            var option = CreateOption(cmdOption);
            options.Add(option);
        }
        
        return options;
    }
    
    private CliCommandAttribute GetCommandInfo(ICliCommand cliCmd)
    {
        var cmdAttr = cliCmd.GetType().GetCustomAttribute<CliCommandAttribute>();

        if (cmdAttr == null)
        {
            throw new CliCommandAttributeNotFound();
        }

        return cmdAttr;
    }

    private Option CreateOption(CliOptionAttribute cmdOption)
    {
        // TODO: using reflection to instantiate the option until generic attributes are implemented
        // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-10.0/generic-attributes
        var cmdOptionType = typeof(Option<>).MakeGenericType(new[] {cmdOption.TOption});
        var option = (Option) Activator.CreateInstance(cmdOptionType, cmdOption.Aliases, cmdOption.Description);
        
        return option;
    }
}
