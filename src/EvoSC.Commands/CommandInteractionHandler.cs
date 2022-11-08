﻿
using EvoSC.Commands.Exceptions;
using EvoSC.Commands.Interfaces;
using EvoSC.Commands.Parser;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Parsing;
using EvoSC.Common.Remote;
using EvoSC.Common.TextParsing;
using EvoSC.Common.TextParsing.ValueReaders;
using GbxRemoteNet.Events;
using Microsoft.Extensions.Logging;
using StringReader = EvoSC.Common.TextParsing.ValueReaders.StringReader;

namespace EvoSC.Commands;

public class CommandInteractionHandler : ICommandInteractionHandler
{
    private readonly ILogger<CommandInteractionHandler> _logger;
    private readonly IEventManager _events;
    private readonly IControllerManager _controllers;
    private readonly IServerClient _serverClient;
    
    private readonly ChatCommandParser _parser;
    
    public CommandInteractionHandler(ILogger<CommandInteractionHandler> logger, IChatCommandManager cmdManager, IEventManager events, IControllerManager controllers, IServerClient serverClient)
    {
        _logger = logger;
        _events = events;
        _controllers = controllers;
        _serverClient = serverClient;
        
        _events.Subscribe(builder => builder
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
        if (result.Exception as CommandParserException is not null && ((CommandParserException)result.Exception).IntendedCommand)
        {
            var message = $"Error: {result.Exception.Message}";
            await _serverClient.Remote.ChatSendServerMessageToLoginAsync($"Error: {message}", playerLogin);
        }
        else
        {
            _logger.LogError("Failed to parse command: {Msg} | Stacktrace: {St}",
                result.Exception.Message,
                result.Exception.StackTrace);
        }
    }
    
    public async Task OnPlayerChatEvent(object sender, PlayerChatEventArgs args)
    {
        // parse
        // execute
        // handle errors

        try
        {
            var parserResult = await _parser.Parse(args.Text);

            if (parserResult.Success)
            {
                var (controller, context) = _controllers.CreateInstance(parserResult.Command.ControllerType);
                var task = (Task)parserResult.Command.HandlerMethod.Invoke(controller,
                    parserResult.Arguments.ToArray());
                await task;
            }
            else if (parserResult.Exception != null)
            {
                await HandleUserErrors(parserResult, args.Login);
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
        }
    }
}
