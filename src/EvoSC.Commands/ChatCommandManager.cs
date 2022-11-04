using System.Reflection;
using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Events;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
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
    public Dictionary<string, IChatCommand> _cmds;
    public Dictionary<string, string> _aliasMap;

    private readonly ILogger<ChatCommandManager> _logger;
    private readonly IEventManager _events;
    private readonly IControllerManager _controllers;

    public ChatCommandManager(ILogger<ChatCommandManager> logger, IEventManager events, IControllerManager controllers)
    {
        _logger = logger;
        _events = events;
        _controllers = controllers;
        _cmds = new Dictionary<string, IChatCommand>();
        _aliasMap = new Dictionary<string, string>();
        
        _events.Subscribe(builder => builder
            .WithEvent(GbxRemoteEvent.PlayerChat)
            .WithInstanceClass<ChatCommandManager>()
            .WithInstance(this)
            .WithHandlerMethod<PlayerChatEventArgs>(OnPlayerChatEvent)
            .AsAsync()
        );
    }

    public async Task OnPlayerChatEvent(object sender, PlayerChatEventArgs args)
    {
        // parse
        // execute
        // handle errors

        if (!args.Text.StartsWith("/"))
        {
            return;
        }

        var cmdArgs = args.Text.Substring(1).Split(" ");
        var cmdName = cmdArgs[0];
        var argValues = new List<object>();

        var valueReader = new ValueReaderManager();
        valueReader.AddReader(new StringReader());
        
        for (int i = 1; i < cmdArgs.Length; i++)
        {
            argValues.Add(await valueReader.ConvertValue(typeof(string), cmdArgs[i]));
        }

        if (!_cmds.ContainsKey(cmdName))
        {
            _logger.LogError("Command not found!");
            return;
        }

        var cmd = _cmds[cmdName];
        var (controller, context) = _controllers.CreateInstance(cmd.ControllerType);
        var task = (Task)cmd.HandlerMethod.Invoke(controller, argValues.ToArray());

        _logger.LogInformation("hello from chat commands manager");
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
                    .WithPermission(cmdAttr.Permission)
                    .WithHandlerMethod(method)
                    .WithController(controllerType);

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
