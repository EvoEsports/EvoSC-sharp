using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ObjectiveC;
using System.Threading.Tasks;
using EvoSC.Core.Commands.Chat;
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

public abstract class CommandsService<TGroup> : ICommandsService
{
    private CommandCollection _commands = new();
    private IServiceProvider _services;

    public CommandCollection Commands => _commands;

    public CommandsService(IServiceProvider services)
    {
        _services = services;
    }

    public async Task RegisterCommands(Type type)
    {
        // group info & group permission
        var groupAttr = type.GetCustomAttribute<CommandGroupAttribute>();
        var groupPermission = type.GetCustomAttribute<PermissionAttribute>();
        var group = groupAttr == null
            ? null
            : new CommandGroupInfo(groupAttr.Name, groupAttr.Description, groupPermission?.Name);

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

            if (_commands.CommandExists(cmdAttr.Name, group?.Name))
            {
                throw new InvalidOperationException($"A command with the name '{cmdAttr.Name}' already exists.");
            }

            // setup permissions
            var cmdPermission = method.GetCustomAttribute<PermissionAttribute>();
            var permission = cmdPermission?.Name ?? groupPermission?.Name ?? null;

            // cmd group
            var cmdGroupAttr = method.GetCustomAttribute<CommandGroupAttribute>();
            var cmdGroup = cmdGroupAttr == null
                ? group
                : new CommandGroupInfo(cmdGroupAttr.Name, cmdGroupAttr.Description, permission);

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
            var command = new Command(method, type, cmdParams, cmdAttr.Name, cmdAttr.Description, permission, cmdGroup);

            _commands.AddCommand(command);
        }
    }

    public Task RegisterCommands<T>() => RegisterCommands(typeof(T));

    public Task UnregisterCommands(Type type)
    {
        var groupAttr = type.GetCustomAttribute<CommandGroupAttribute>();
        
        var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

        foreach (var method in methods)
        {
            var cmdAttr = method.GetCustomAttribute<CommandAttribute>();

            if (cmdAttr == null)
            {
                continue;
            }
            
            var cmdGroupAttr = method.GetCustomAttribute<CommandGroupAttribute>();
            var cmdGroup = cmdGroupAttr ?? groupAttr;

            if (_commands.CommandExists(cmdAttr.Name, cmdGroup?.Name))
            {
                _commands.RemoveCommand(cmdAttr.Name, cmdGroup?.Name);
            }
        }

        return Task.CompletedTask;
    }

    public Task UnregisterCommands<T>() => UnregisterCommands(typeof(T));

    public Task<ICommandResult> ExecuteCommand(ICommandContext context, ICommandParserResult parserResult)
    {
        if (parserResult.Command.Permission == null || context.Player.HasPermission(parserResult.Command.Permission))
        {
            return parserResult.Command.Invoke(_services, context, parserResult.Arguments.ToArray());
        }

        var result = new CommandResult(false, new CommandException("Permission denied."));
        return Task.FromResult((ICommandResult)result);
    }

    public ICommand GetCommand(string group, string? name = null)
    {
        if (_commands.CommandExists(name, group))
        {
            return _commands.GetCommand(name, group);
        } 
        else if (_commands.CommandExists(group))
        {
            return _commands.GetCommand(group);
        }
        
        throw new CommandException($"Command '{group}' not found");
    }
}
