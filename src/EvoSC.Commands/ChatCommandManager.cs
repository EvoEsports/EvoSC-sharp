using System.Reflection;
using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Events;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Util;

namespace EvoSC.Commands;

public class ChatCommandManager : IChatCommandManager
{
    public Dictionary<string, IChatCommand> _cmds;
    public Dictionary<string, string> _aliasMap;

    public ChatCommandManager()
    {
        _cmds = new Dictionary<string, IChatCommand>();
    }

    public void RegisterForController(Type controllerType)
    {
        var methods = controllerType.GetMethods(ReflectionUtils.InstanceMethods);

        foreach (var method in methods)
        {
            var cmdAttr = method.GetCustomAttribute<ChatCommandAttribute>();

            if (cmdAttr == null)
            {
                continue;
            }

            var aliasAttrs = method.GetCustomAttributes<CommandAliasAttribute>();

            AddCommand(builder =>
            {
                builder
                    .WithName(cmdAttr.Name)
                    .WithDescription(cmdAttr.Description)
                    .WithPermission(cmdAttr.Permission);

                foreach (var alias in aliasAttrs)
                {
                    builder.AddAlias(alias.Alias);
                }
            });
        }
    }

    public void AddCommand(IChatCommand cmd)
    {
        if (_cmds.ContainsKey(cmd.Name))
        {
            throw new InvalidOperationException($"Chat command with name '{cmd.Name}' already exists.");
        }
        
        foreach (var alias in cmd.Aliases)
        {
            if (_aliasMap.ContainsKey(alias))
            {
                throw new InvalidOperationException(
                    $"Chat command {cmd.Name} tried to register an alias '{alias}' which already exists.");
            }

            _aliasMap[alias] = cmd.Name;
        }

        _cmds[cmd.Name] = cmd;
    }

    public void AddCommand(Action<ChatCommandBuilder> builderAction)
    {
        var builder = new ChatCommandBuilder();
        builderAction(builder);
        AddCommand(builder.Build());
    }
}
