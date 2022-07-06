using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using EvoSC.Attributes;
using EvoSC.Domain.Commands;
using EvoSC.Domain.Players;
using EvoSC.Interfaces.Commands;

namespace EvoSC.Core.Services.Commands;

public class CommandService : ICommandService
{
    private readonly Dictionary<string, Command> _commands = new();

    public Task ClientOnPlayerChatCommand(Player player, Command command)
    {
        throw new NotImplementedException();
    }

    public void RegisterCommands(Type type)
    {
        var methods = type.GetMethods();

        foreach (var method in methods)
        {
            // if it annotates the CommandAttribute attribute, get Command info and register it
            var cmdAttr = method.GetCustomAttribute<CommandAttribute>();
            if (cmdAttr == null)
            {
                continue;
            }

            var permsAttr = method.GetCustomAttribute<PermissionAttribute>();
            var parameters = new Dictionary<string, string>();
            foreach (var param in method.GetParameters())
            {
                var paramDesc = param.GetCustomAttribute<DescriptionAttribute>()?.Description ?? "No description";

                parameters.Add(param.Name, paramDesc);
            }

            var cmd = new Command
            {
                CmdMethod = method,
                CmdType = type,
                Description = cmdAttr.Description,
                Permission = permsAttr?.PermissionName,
                Name = cmdAttr.Name,
                Parameters = parameters,
            };

            _commands.Add(cmd.Name, cmd);
        }
    }
}
