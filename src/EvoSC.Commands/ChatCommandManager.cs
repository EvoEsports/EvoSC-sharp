using System.Reflection;
using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Events;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Remote;
using EvoSC.Common.Util;
using GbxRemoteNet.Events;
using Microsoft.Extensions.Logging;

namespace EvoSC.Commands;

public class ChatCommandManager : IChatCommandManager
{
    public Dictionary<string, IChatCommand> _cmds;
    public Dictionary<string, string> _aliasMap;

    private readonly ILogger<ChatCommandManager> _logger;
    private readonly IEventManager _events;

    public ChatCommandManager(ILogger<ChatCommandManager> logger, IEventManager events)
    {
        _logger = logger;
        _events = events;
        _cmds = new Dictionary<string, IChatCommand>();
        
        _events.Subscribe(builder => builder
            .WithName(GbxRemoteEvent.PlayerChat)
            .WithInstanceClass<ChatCommandManager>()
            .WithInstance(this)
            .WithHandlerMethod<PlayerChatEventArgs>(OnPlayerChatEvent)
        );
    }

    public Task OnPlayerChatEvent(object sender, PlayerChatEventArgs args)
    {
        _logger.LogInformation("hello from chat commands manager");
        return Task.CompletedTask;
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

        
        MapCommandAliases(cmd);
        _cmds[cmd.Name] = cmd;
    }

    private void MapCommandAliases(IChatCommand cmd)
    {
        if (cmd.Aliases != null)
        {
            foreach (var alias in cmd.Aliases)
            {
                if (_aliasMap.ContainsKey(alias))
                {
                    throw new InvalidOperationException(
                        $"Chat command {cmd.Name} tried to register an alias '{alias}' which already exists.");
                }

                _aliasMap[alias] = cmd.Name;
            }
        }
    }

    public void AddCommand(Action<ChatCommandBuilder> builderAction)
    {
        var builder = new ChatCommandBuilder();
        builderAction(builder);
        AddCommand(builder.Build());
    }
}
