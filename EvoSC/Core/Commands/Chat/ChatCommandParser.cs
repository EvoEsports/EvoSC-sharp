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

public class ChatCommandParser : ICommandParser
{
    private readonly IChatCommandsService _chatCommands;
    private readonly ValueReaderManager _valueReader;
    
    public ChatCommandParser(IChatCommandsService chatCommands, ValueReaderManager valueReader)
    {
        _chatCommands = chatCommands;
        _valueReader = valueReader;
    }
    
    public async Task<ICommandParserResult> Parse(string input)
    {
        try
        {
            input = input.TrimStart();

            // remove slashes
            var i = 0;
            while (input[i] == '/')
            {
                input = input[(i + 1)..];
            }

            // check command and parameters
            var parts = input.Split(' ');

            if (parts.Length < 1)
            {
                return new ChatCommandParserResult(false, null, Array.Empty<object>(),
                    new CommandException("Not a command. Provide a name."));
            }

            var cmdName = parts[0];
            var command = _chatCommands.GetCommand(cmdName, ""); // todo: implement group/sub commands
            var numInputArgs = parts.Length - 1;

            if (numInputArgs - 1 < command.RequiredParameters())
            {
                return new ChatCommandParserResult(false, command, Array.Empty<object>(),
                    new CommandException("Missing parameters"));
            }

            // combine last parameter if string
            if (numInputArgs > command.Parameters.Length && command.Parameters.Last().ParameterType == typeof(string))
            {
                parts[numInputArgs - 1] = string.Join(' ', parts[(numInputArgs - 1)..]);
            }

            // convert args
            var args = new List<object>();

            try
            {
                for (i = 0; i < command.Parameters.Length; i++)
                {
                    var inputParam = parts[i + 1];
                    var paramInfo = command.Parameters[i];

                    var argValue = _valueReader.ConvertValue(paramInfo.ParameterType, inputParam);

                    // value converted, add it
                    args.Add(argValue);
                }
            }
            catch (Exception ex)
            {
                return new ChatCommandParserResult(false, command, Array.Empty<object>(),
                    new CommandException("Invalid arguments to command: " + ex.Message));
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
