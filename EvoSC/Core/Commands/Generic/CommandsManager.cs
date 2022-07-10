using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using EvoSC.Core.Commands.Generic.Attributes;
using EvoSC.Core.Commands.Generic.Exceptions;
using EvoSC.Core.Commands.Generic.Interfaces;
using EvoSC.Domain;
using EvoSC.Domain.Groups;
using EvoSC.Domain.Players;
using EvoSC.Interfaces.Messages;
using EvoSC.Interfaces.Commands;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EvoSC.Core.Commands.Generic;

public abstract class CommandsManager<TGroupType> : ICommandsService
{
    private CommandCollection _commands = new();
    private IServiceProvider _services;

    public CommandsManager(IServiceProvider services)
    {
        _services = services;
    }

    public async Task RegisterCommands(Type type)
    {
        // group info & group permission
        var groupAttr = type.GetCustomAttribute<CommandGroupAttribute>();
        var groupPermission = type.GetCustomAttribute<PermissionAttribute>();

        // commands
        var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public);

        foreach (var method in methods)
        {
            var cmdAttr = method.GetCustomAttribute<CommandAttribute>();

            if (cmdAttr == null)
            {
                // ignore a non-cmd method
                continue;
            }

            // setup permissions
            var cmdPermission = method.GetCustomAttribute<PermissionAttribute>();
            var permission = cmdPermission ?? groupPermission ?? null;

            // parameters
            var parameters = method.GetParameters();
            List<ICommandParameter> cmdParams = new();

            foreach (var param in parameters)
            {
                var description = param.GetCustomAttribute<DescriptionAttribute>();
                var cmdParam = new CommandParameter(param.ParameterType, param.Name, param.IsOptional,
                    description?.Description);
                cmdParams.Add(cmdParam);
            }

            // register the command
            var command = new Command(method, type, cmdParams, cmdAttr.Name, cmdAttr.Description, permission?.Name,
                groupAttr?.Name);

            _commands.Add(command);
        }
    }

    public Task RegisterCommands<T>() => RegisterCommands(typeof(T));

    public Task UnregisterCommands(Type type)
    {
        return Task.CompletedTask;
    }

    public Task UnregisterCommands<T>() => UnregisterCommands(typeof(T));

    public Task<ICommandResult> ExecuteCommand(ICommandContext context, ICommandParserResult parserResult)
    {
        // todo: check permissions
        if (!context.Player.HasPermission(parserResult.Command.Permission))
        {
            var result = new CommandResult(false, new CommandException("Permission denied."));
            return Task.FromResult((ICommandResult)result);
        }

        return parserResult.Command.Invoke(_services, context, parserResult.Arguments.ToArray());
    }

    public ICommand GetCommand(string group, string name)
    {
        if (_commands.TryGetValue(group, out var command))
        {
            return command;
        }

        throw new CommandException($"Command '{group}' not found");
    }
}
