using System.CommandLine;
using System.CommandLine.Invocation;
using System.ComponentModel;
using System.Reflection;
using EvoSC.CLI.Attributes;
using EvoSC.CLI.Interfaces;
using EvoSC.CLI.Models;
using EvoSC.Common.Application;
using EvoSC.Common.Config;
using EvoSC.Common.Util;
using EvoSC.Common.Util.EnumIdentifier;
using Microsoft.Extensions.DependencyInjection;

namespace EvoSC.CLI;

public class CliManager : ICliManager
{
    private readonly RootCommand _rootCommand;
    private readonly Option<CliEvoScConfig> _configOption;
    private readonly List<ICliCommandInfo> _registeredCommands = new();

    public CliManager()
    {
        _rootCommand = new RootCommand("EvoSC# TrackMania server controller.");

        _configOption =
            new Option<CliEvoScConfig>(new[] {"--option", "-o"},
                description: "Override configuration options. Format is key:value");
        
        _rootCommand.AddGlobalOption(_configOption);
    }
    
    public ICliManager RegisterCommands(Assembly assembly)
    {
        foreach (var cmdClass in assembly.AssemblyTypesWithAttribute<CliCommandAttribute>())
        {
            var handlerMethod = cmdClass.GetInstanceMethod("ExecuteAsync");
            
            var cmdAttr = cmdClass.GetCustomAttribute<CliCommandAttribute>();
            var requiredFeatures = cmdClass.GetCustomAttribute<RequiredFeaturesAttribute>();

            var command = new Command(cmdAttr!.Name, cmdAttr.Description);

            var options = new List<Option>();
            foreach (var param in handlerMethod.GetParameters())
            {
                var option = CreateOption(param);

                if (option == null)
                {
                    throw new InvalidOperationException($"Failed to create option for CLI command {cmdClass.Name}");
                }

                options.Add(option);
                command.AddOption(option);
            }
            
            options.Add(_configOption);

            var commandInfo = new CliCommandInfo
            {
                Command = command,
                CommandClass = cmdClass,
                HandlerMethod = handlerMethod,
                Options = options.ToArray(),
                RequiredFeatures = requiredFeatures?.Features ?? Array.Empty<AppFeature>()
            };

            command.SetHandler(async context =>
            {
                await ExecuteHandlerAsync(commandInfo, context);
            });

            _registeredCommands.Add(commandInfo);
        }

        return this;
    }

    private async Task ExecuteHandlerAsync(ICliCommandInfo command, InvocationContext context)
    {
        var paramValues = new List<object?>();

        foreach (var option in command.Options)
        {
            var optionValue = context.BindingContext.ParseResult.GetValueForOption(option);
            paramValues.Add(optionValue);
        }

        var cliConfig = context.BindingContext.ParseResult.GetValueForOption(_configOption);
        var config = Configuration.GetBaseConfig(cliConfig?.Options ?? new Dictionary<string, object>());

        var startupPipeline = new StartupPipeline(config);
        startupPipeline.SetupBasePipeline(config);

        await startupPipeline.ExecuteAsync(command.RequiredFeatures
            .Select(feature => feature.ToString())
            .ToArray()
        );

        var cmdObject = ActivatorUtilities.CreateInstance(startupPipeline.ServiceContainer, command.CommandClass);

        var task = ReflectionUtils.CallMethod(cmdObject, "ExecuteAsync", paramValues) as Task;

        if (task == null)
        {
            throw new InvalidOperationException("Failed to call CLI command handler as the task is null.");
        }

        await task;
    }

    private Option? CreateOption(ParameterInfo param)
    {
        var cmdOptionType = typeof(Option<>).MakeGenericType(new[] {param.ParameterType});
        var aliases = cmdOptionType
            .GetCustomAttributes<AliasAttribute>()
            .Select(a => a.Name)
            .ToList();

        aliases.Add($"--{param.Name}");

        var description = param.GetCustomAttribute<DescriptionAttribute>();

        var option = Activator.CreateInstance(cmdOptionType, aliases, description?.Description ?? "") as Option;

        return option;
    }

    public Task<int> ExecuteAsync(string[] args)
    {
        throw new NotImplementedException();
    }
}
