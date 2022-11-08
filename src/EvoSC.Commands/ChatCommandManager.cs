using System.Reflection;
using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Commands.Parser;
using EvoSC.Common.Events;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Parsing;
using EvoSC.Common.Remote;
using EvoSC.Common.TextParsing;
using EvoSC.Common.TextParsing.ValueReaders;
using EvoSC.Common.Util;
using GbxRemoteNet.Events;
using Microsoft.Extensions.Logging;
using StringReader = EvoSC.Common.TextParsing.ValueReaders.StringReader;

namespace EvoSC.Commands;

public class ChatCommandManager : IChatCommandManager
{
    public const string CommandPrefix = "/";
    
    public Dictionary<string, IChatCommand> _cmds;
    public Dictionary<string, string> _aliasMap;

    private readonly ILogger<ChatCommandManager> _logger;


    public ChatCommandManager(ILogger<ChatCommandManager> logger)
    {
        _logger = logger;
        _cmds = new Dictionary<string, IChatCommand>();
        _aliasMap = new Dictionary<string, string>();
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
            var prefix = cmdAttr.UsePrefix ? CommandPrefix : "";

            AddCommand(builder =>
            {
                builder
                    .WithName(cmdAttr.Name)
                    .WithDescription(cmdAttr.Description)
                    .WithPermission(cmdAttr.Permission)
                    .WithHandlerMethod(method)
                    .WithController(controllerType)
                    .UsePrefix(cmdAttr.UsePrefix);

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

        var lookupName = (cmd.UsePrefix ? CommandPrefix : "") + cmd.Name;
        
        MapCommandAliases(cmd);
        _cmds[lookupName] = cmd;
    }

    private void MapCommandAliases(IChatCommand cmd)
    {
        if (cmd.Aliases != null)
        {
            var prefix = cmd.UsePrefix ? CommandPrefix : "";
            foreach (var alias in cmd.Aliases)
            {
                if (_aliasMap.ContainsKey(alias))
                {
                    throw new InvalidOperationException(
                        $"Chat command {cmd.Name} tried to register an alias '{alias}' which already exists.");
                }

                _aliasMap[alias] = prefix + cmd.Name;
            }
        }
    }

    public void AddCommand(Action<ChatCommandBuilder> builderAction)
    {
        var builder = new ChatCommandBuilder();
        builderAction(builder);
        AddCommand(builder.Build());
    }

    public IChatCommand FindCommand(string alias, bool withPrefix=true)
    {
        var lookupName = (withPrefix ? CommandPrefix : "") + alias;
        
        if (_cmds.ContainsKey(lookupName))
        {
            return _cmds[lookupName];
        }

        if (_aliasMap.ContainsKey(alias))
        {
            return _cmds[_aliasMap[alias]];
        }

        return null;
    }
}
