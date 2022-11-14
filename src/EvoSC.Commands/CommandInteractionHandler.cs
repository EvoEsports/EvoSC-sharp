
using EvoSC.Commands.Exceptions;
using EvoSC.Commands.Interfaces;
using EvoSC.Commands.Parser;
using EvoSC.Common.Controllers.Context;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Middleware;
using EvoSC.Common.Interfaces.Models.Enums;
using EvoSC.Common.Interfaces.Parsing;
using EvoSC.Common.Models;
using EvoSC.Common.Remote;
using EvoSC.Common.TextParsing;
using EvoSC.Common.TextParsing.ValueReaders;
using EvoSC.Common.Util;
using GbxRemoteNet.Events;
using Microsoft.Extensions.Logging;
using StringReader = EvoSC.Common.TextParsing.ValueReaders.StringReader;

namespace EvoSC.Commands;

public class CommandInteractionHandler : ICommandInteractionHandler
{
    private readonly ILogger<CommandInteractionHandler> _logger;
    private readonly IControllerManager _controllers;
    private readonly IServerClient _serverClient;
    private readonly IActionPipelineManager _actionPipeline;
    
    private readonly ChatCommandParser _parser;

    public CommandInteractionHandler(ILogger<CommandInteractionHandler> logger, IChatCommandManager cmdManager,
        IEventManager events, IControllerManager controllers, IServerClient serverClient,
        IActionPipelineManager actionPipeline)
    {
        _logger = logger;
        _controllers = controllers;
        _serverClient = serverClient;
        _actionPipeline = actionPipeline;

        events.Subscribe(builder => builder
            .WithEvent(GbxRemoteEvent.PlayerChat)
            .WithInstanceClass<ChatCommandManager>()
            .WithInstance(this)
            .WithHandlerMethod<PlayerChatEventArgs>(OnPlayerChatEvent)
            .AsAsync()
        );

        _parser = new ChatCommandParser(cmdManager, GetValueReader());
    }

    private IValueReaderManager GetValueReader()
    {
        var valueReader = new ValueReaderManager();
        
        valueReader.AddReader(new FloatReader());
        valueReader.AddReader(new IntegerReader());
        valueReader.AddReader(new StringReader());

        return valueReader;
    }

    async Task HandleUserErrors(IParserResult result, string playerLogin)
    {
        if (result.Exception as CommandParserException is not null)
        {
            if (!((CommandParserException)result.Exception).IntendedCommand)
            {
                return;
            }
            
            var message = $"Error: {result.Exception.Message}";
            await _serverClient.SendChatMessage($"Error: {message}", playerLogin);
        }
        else
        {
            _logger.LogError("Failed to parse command: {Msg} | Stacktrace: {St}",
                result.Exception.Message,
                result.Exception.StackTrace);
        }
    }

    private async Task ExecuteCommand(IChatCommand cmd, object[] args, PlayerChatEventArgs eventArgs)
    {
        var (controller, context) = _controllers.CreateInstance(cmd.ControllerType);
        
        // todo: use player service for this instead
        var onlinePlayerInfo = await _serverClient.Remote.GetDetailedPlayerInfoAsync(eventArgs.Login);
        var player = new OnlinePlayer
        {
            AccountId = PlayerUtils.ConvertLoginToAccountId(eventArgs.Login),
            NickName = onlinePlayerInfo.NickName,
            UbisoftName = onlinePlayerInfo.NickName,
            Zone = onlinePlayerInfo.Path,
            State = onlinePlayerInfo.IsSpectator ? PlayerState.Spectating : PlayerState.Playing
        };

        var playerInteractionContext = new CommandInteractionContext(player, context) {CommandExecuted = cmd};
        controller.SetContext(playerInteractionContext);

        var actionChain = _actionPipeline.BuildChain(context =>
        {
            return (Task)cmd.HandlerMethod.Invoke(controller, args);
        });

        await actionChain(playerInteractionContext);
    }
    
    public async Task OnPlayerChatEvent(object sender, PlayerChatEventArgs eventArgs)
    {
        // parse
        // execute
        // handle errors

        try
        {
            var parserResult = await _parser.Parse(eventArgs.Text);

            if (parserResult.Success)
            {
                await ExecuteCommand(parserResult.Command, parserResult.Arguments.ToArray(), eventArgs);
            }
            else if (parserResult.Exception != null)
            {
                await HandleUserErrors(parserResult, eventArgs.Login);
            }
            else
            {
                _logger.LogError(
                    "An unknown error occured while trying to parse command. No exception thrown, but parsing failed.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogCritical("A fatal error occured while trying to handle chat command: {Msg} | Stacktrace: {St}",
                ex.Message, ex.StackTrace);
            throw;
        }
    }
}
