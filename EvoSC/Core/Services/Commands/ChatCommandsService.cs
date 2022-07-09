using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using EvoSC.Core.Commands.Chat;
using EvoSC.Core.Commands.Generic;
using EvoSC.Core.Commands.Generic.Attributes;
using EvoSC.Core.Commands.Generic.Exceptions;
using EvoSC.Core.Commands.Generic.Parsing;
using EvoSC.Core.Commands.Generic.Parsing.Readers;
using EvoSC.Domain.Players;
using EvoSC.Interfaces.Chat;
using EvoSC.Interfaces.Commands;
using GbxRemoteNet;
using NLog;

namespace EvoSC.Core.Services.Commands;

public class ChatCommandsService : CommandsManager<ChatCommandGroup>, IChatCommandsService
{
    private readonly Logger _logger = LogManager.GetCurrentClassLogger();
    private readonly GbxRemoteClient _gbxClient;
    private readonly ValueReaderManager _valueReader;
    
    public ChatCommandsService(IServiceProvider services, GbxRemoteClient gbxClient) : base(services)
    {
        _gbxClient = gbxClient;

        // setup value reader for chat commands
        _valueReader = new ValueReaderManager();
        SetupValueReader();
    }

    private void SetupValueReader()
    {
        _valueReader.AddReader(new FloatReader());
        _valueReader.AddReader(new IntegerReader());
        _valueReader.AddReader(new StringReader());
    }

    public async Task OnChatMessage(IServerServerChatMessage message)
    {
        if (message.IsCommand)
        {
            // parse the command
            var parser = new ChatCommandParser(this, _valueReader);
            var parserResult = await parser.Parse(message.Content);

            if (!parserResult.IsSuccess)
            {
                ReplyError(parserResult.Exception, message);
                return;
            }

            // execute the parsed command
            var commandContext = new ChatCommandContext(_gbxClient, message);
            var executionResult = await ExecuteCommand(commandContext, parserResult);

            if (!executionResult.IsSuccess)
            {
                ReplyError(executionResult.Exception, message);
                return;
            }
        }
    }
    
    public async Task ReplyError(Exception ex, IServerServerChatMessage message)
    {
        if (ex.GetType() == typeof(CommandException))
        {
            // command exceptions are meant to hold user-feedback, so send the error message to the user
            await message.ReplyAsync("Command failed: " + ex.Message);
        }
        else
        {
            await message.ReplyAsync("Command failed: unknown error");

            _logger.Error("Failed to execute command: {Msg} | Stacktrace: {St}",
                ex.Message, ex.StackTrace);
        }
    }
}
