﻿using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.ComponentModel;
using System.Reflection;
using EvoSC.CLI.Attributes;
using EvoSC.CLI.Exceptions;
using EvoSC.CLI.Interfaces;
using EvoSC.CLI.Models;
using EvoSC.Common.Application;
using EvoSC.Common.Config;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Services;
using EvoSC.Common.Util;
using EvoSC.Common.Util.EnumIdentifier;
using Microsoft.Extensions.DependencyInjection;
using Container = SimpleInjector.Container;

namespace EvoSC.CLI;

public class CliManager : ICliManager, IEvoSCApplication
{
    private readonly RootCommand _rootCommand;
    private readonly Parser _cliParser;
    private readonly Option<IEnumerable<string>> _configOption;
    private readonly List<ICliCommandInfo> _registeredCommands = new();

    public CliManager()
    {
        _rootCommand = new RootCommand("EvoSC# TrackMania server controller.");

        _configOption =
            new Option<IEnumerable<string>>(new[] {"--option", "-o"},
                description: "Override configuration options. Format is key:value");
        
        _rootCommand.AddGlobalOption(_configOption);

        _cliParser = new CommandLineBuilder(_rootCommand)
            .UseVersionOption()
            .UseHelp()
            .UseEnvironmentVariableDirective()
            .UseParseDirective()
            .UseSuggestDirective()
            .RegisterWithDotnetSuggest()
            .UseTypoCorrections()
            .UseParseErrorReporting()
            .CancelOnProcessTermination()
            .Build();
    }

    private async Task ExecuteHandlerAsync(ICliCommandInfo command, InvocationContext context)
    {
        var paramValues = new List<object?>();

        foreach (var option in command.Options)
        { 
            if (option == _configOption)
            {
                continue;
            }

            if (option.ValueType == typeof(InvocationContext))
            {
                paramValues.Add(context);
                continue;
            }
            
            var optionValue = context.BindingContext.ParseResult.GetValueForOption(option);
            paramValues.Add(optionValue);
        }

        var cliConfig = context.BindingContext.ParseResult.GetValueForOption(_configOption);
        var cliConfigOptions = new Dictionary<string, string> ();

        if (cliConfig != null)
        {
            foreach (var cliConfigOption in cliConfig)
            {
                var kv = cliConfigOption.Split(':', 2);

                if (kv.Length < 2)
                {
                    throw new InvalidOperationException(
                        $"The provided config option {cliConfigOption} is in an invalid format.");
                }

                cliConfigOptions[kv[0]] = kv[1];
            }
        }

        var config = Configuration.GetBaseConfig(cliConfigOptions);

        var startupPipeline = new StartupPipeline(config);
        startupPipeline.ServiceContainer.ConfigureServiceContainerForEvoSc();
        startupPipeline.SetupBasePipeline(config);
        startupPipeline.Services("CliContext", s => s.RegisterInstance<ICliContext>(new CliContext(context)));
        startupPipeline.Services("Application", s => s.RegisterInstance<IEvoSCApplication>(this));

        await startupPipeline.ExecuteAsync(command.RequiredFeatures
            .Select(feature => feature.ToString())
            .ToArray()
        );

        var cmdObject = ActivatorUtilities.CreateInstance(startupPipeline.ServiceContainer, command.CommandClass);

        var methodArgs = paramValues?.ToArray() ?? Array.Empty<object>();
        var task = ReflectionUtils.CallMethod(cmdObject, "ExecuteAsync", methodArgs) as Task;

        if (task == null)
        {
            throw new InvalidOperationException("Failed to call CLI command handler as the task is null.");
        }

        await task;
    }

    private Option? CreateOption(ParameterInfo param)
    {
        var cmdOptionType = typeof(Option<>).MakeGenericType(param.ParameterType);
        var aliases = param
            .GetCustomAttributes<AliasAttribute>()
            .Select(a => a.Name)
            .ToList();

        aliases.Add($"--{param.Name}");

        var description = param.GetCustomAttribute<DescriptionAttribute>();

        var option = Activator.CreateInstance(cmdOptionType, aliases.ToArray(), description?.Description ?? "") as Option;

        return option;
    }
    
    public ICliManager RegisterCommands(Assembly assembly)
    {
        foreach (var cmdClass in assembly.AssemblyTypesWithAttribute<CliCommandAttribute>())
        {
            RegisterCommand(cmdClass);
        }

        return this;
    }

    public ICliManager RegisterCommand(Type cmdClass)
    {
        var handlerMethod = cmdClass.GetMethod("ExecuteAsync");

        if (handlerMethod == null)
        {
            throw new InvalidCommandClassFormatException(
                $"Did not find the ExecuteAsync method in CLI command: {cmdClass.Name}");
        }
        
        var cmdAttr = cmdClass.GetCustomAttribute<CliCommandAttribute>();

        if (cmdAttr == null)
        {
            throw new InvalidCommandClassFormatException("Missing CliCommand");
        }
        
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
        _rootCommand.AddCommand(command);

        return this;
    }

    public Task<int> ExecuteAsync(string[] args)
    {
        return _cliParser.InvokeAsync(args);
    }

    public IStartupPipeline StartupPipeline { get; }
    public CancellationToken MainCancellationToken { get; }
    public Container Services => StartupPipeline.ServiceContainer;
    
    public Task RunAsync()
    {
        throw new NotImplementedException();
    }

    public Task ShutdownAsync()
    {
        throw new NotImplementedException();
    }
}
