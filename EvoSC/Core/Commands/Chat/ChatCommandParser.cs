using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EvoSC.Core.Commands.Generic.Exceptions;
using EvoSC.Core.Commands.Generic.Interfaces;
using EvoSC.Core.Commands.Generic.Parsing;
using EvoSC.Interfaces.Commands;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EvoSC.Core.Commands.Chat;

public class ChatCommandParser : ICommandParser<ChatCommandParserResult>
{
    private readonly IChatCommandsService _chatCommands;
    private readonly ValueReaderManager _valueReader;

    public ChatCommandParser(IChatCommandsService chatCommands, ValueReaderManager valueReader)
    {
        _chatCommands = chatCommands;
        _valueReader = valueReader;
    }

    public async Task<ChatCommandParserResult> Parse(string input)
    {
        try
        {
            input = input.TrimStart();

            // remove slashes
            var i = 0;
            while (input[i] == '/')
            {
                input = input[(i + 1)..];
                i++;
            }

            // check command and parameters
            var parts = input.Split(' ');

            if (parts.Length < 1)
            {
                return new ChatCommandParserResult(false, null, Array.Empty<object>(),
                    new CommandException("Not a command. Provide a name."));
            }

            var groupName = parts[0];
            var cmdName = parts.Length > 1 ? parts[1] : null;
            var command = _chatCommands.GetCommand(groupName, cmdName);

            if (command.Group != null)
            {
                parts = parts[1..];
            }

            var numInputArgs = parts.Length - 1;

            if (numInputArgs < command.RequiredParameters())
            {
                return new ChatCommandParserResult(false, command, Array.Empty<object>(),
                    new CommandException("Missing parameters"));
            }

            // combine last parameter if string
            if (command.Parameters.Length > 0 && numInputArgs > command.Parameters.Length &&
                command.Parameters.Last().ParameterType == typeof(string))
            {
                parts[numInputArgs - 1] = string.Join(' ', parts[(numInputArgs - 1)..]);
            }

            // convert args
            var args = new List<object>();

            for (i = 0; i < command.Parameters.Length; i++)
            {
                var inputParam = parts[i + 1];
                var paramInfo = command.Parameters[i];

                try
                {
                    var argValue = await _valueReader.ConvertValue(paramInfo.ParameterType, inputParam);

                    // value converted, add it
                    args.Add(argValue);
                }
                catch (FormatException)
                {
                    return new ChatCommandParserResult(false, command, Array.Empty<object>(),
                        new CommandException($"Argument '{paramInfo.Name}' is in an invalid format."));
                }
            }

            return new ChatCommandParserResult(true, command, args, null);
        }
        catch (CommandException ex)
        {
            return new ChatCommandParserResult(false, null, Array.Empty<object>(),
                new CommandException(ex.Message));
        }
        catch (Exception ex)
        {
            return new ChatCommandParserResult(false, null, Array.Empty<object>(), ex);
        }
    }
}
