using System.Windows.Input;
using EvoSC.Commands.Exceptions;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Interfaces.Parsing;

namespace EvoSC.Commands.Parser;

public class ChatCommandParser
{
    private readonly IChatCommandManager _cmdManager;
    private readonly IValueReaderManager _valueReader;

    public ChatCommandParser(IChatCommandManager cmdManager, IValueReaderManager valueReader)
    {
        _cmdManager = cmdManager;
        _valueReader = valueReader;
    }

    public async Task<IParserResult> Parse(string userInput)
    {
        try
        {
            var parts = GetInputParts(userInput);

            if (parts.Length == 0)
            {
                throw new InvalidCommandFormatException(false);
            }

            var cmdAlias = parts[0];
            bool intendedCommand = cmdAlias.StartsWith("/");
            var cmd = _cmdManager.FindCommand(cmdAlias, false);

            if (cmd == null)
            {
                throw new CommandNotFoundException(cmdAlias, intendedCommand);
            }

            parts = parts[1..];
            var args = await ConvertArguments(cmd, parts, intendedCommand);

            return new ParserResult(cmd, args, true);
        }
        catch (Exception ex)
        {
            return new ParserResult(null, null, false, ex);
        }
    }

    private async Task<List<object>> ConvertArguments(IChatCommand cmd, string[] parts, bool intendedCommand)
    {
        var requiredCount = cmd.Parameters.Count(p => !p.Optional);
        
        if (requiredCount > parts.Length)
        {
            throw new NotEnoughArgumentsException(requiredCount, intendedCommand);
        }

        // combine last parameters if the input args are bigger than par count and last par is string
        if (parts.Length > cmd.Parameters.Length && cmd.Parameters.Last().Type == typeof(string))
        {
            parts[cmd.Parameters.Length - 1] = string.Join(' ', parts[(cmd.Parameters.Length - 1)..]);
        }

        var convertedArgs = new List<object>();
        
        for (int i = 0; i < cmd.Parameters.Length; i++)
        {
            var parameter = cmd.Parameters[i];
            try
            {
                var value = await _valueReader.ConvertValue(parameter.Type, parts[i]);
                convertedArgs.Add(value);
            }
            catch (FormatException e)
            {
                throw new InvalidCommandArgumentException(parameter.Type.Name ?? "<unknown>", intendedCommand);
            }
        }

        return convertedArgs;
    }

    private static string[] GetInputParts(string userInput)
    {
        userInput = userInput.TrimStart();
        var parts = userInput.Split(" ");
        return parts;
    }
}
