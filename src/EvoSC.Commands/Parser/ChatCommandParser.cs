using EvoSC.Commands.Exceptions;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Interfaces.Parsing;

namespace EvoSC.Commands.Parser;

public class ChatCommandParser
{
    private readonly IChatCommandManager _cmdManager;
    private readonly IValueReaderManager _valueReader;

    public static string CommandPrefix = "/";

    public ChatCommandParser(IChatCommandManager cmdManager, IValueReaderManager valueReader)
    {
        _cmdManager = cmdManager;
        _valueReader = valueReader;
    }

    public async Task<IParserResult> ParseAsync(string userInput)
    {
        try
        {
            var parts = GetInputParts(userInput);

            if (parts.Length == 0)
            {
                throw new InvalidCommandFormatException(false);
            }

            var cmdAlias = parts[0];
            bool intendedCommand = cmdAlias.StartsWith(CommandPrefix, StringComparison.Ordinal);
            var cmd = _cmdManager.FindCommand(cmdAlias, false);

            if (cmd == null)
            {
                throw new CommandNotFoundException(cmdAlias, intendedCommand);
            }

            parts = parts[1..];
            var args = await ConvertArgumentsAsync(cmd, parts, intendedCommand ? null : cmdAlias);

            return new ParserResult {Command = cmd, Arguments = args, Success = true, IsIntended = intendedCommand, AliasUsed = cmdAlias};
        }
        catch (Exception ex)
        {
            return new ParserResult {Command = null, Arguments = null, Success = false, Exception = ex, IsIntended = false};
        }
    }

    private async Task<List<object>> ConvertArgumentsAsync(IChatCommand cmd, string[] parts, string aliasName=null)
    {
        var requiredCount = cmd.Parameters.Count(p => !p.Optional);
        var aliasArgCount = 0;
        var convertedArgs = new List<object>();

        if (aliasName != null)
        {
            // pre-add alias's default arguments
            var alias = cmd.Aliases[aliasName];
            convertedArgs.AddRange(alias.DefaultArgs);
            aliasArgCount = alias.DefaultArgs.Length;
        }
        
        requiredCount -= aliasArgCount;
        
        if (requiredCount > parts.Length)
        {
            throw new NotEnoughArgumentsException(requiredCount, true);
        }

        // combine last parameters if the input args are bigger than par count and last par is string
        if (parts.Length > cmd.Parameters.Length && cmd.Parameters.Last().Type == typeof(string))
        {
            parts[cmd.Parameters.Length - 1] = string.Join(' ', parts[(cmd.Parameters.Length - 1)..]);
        }

        for (int i = aliasArgCount; i < cmd.Parameters.Length; i++)
        {
            var parameter = cmd.Parameters[i];
            try
            {
                var value = await _valueReader.ConvertValueAsync(parameter.Type, parts[i - aliasArgCount]);
                convertedArgs.Add(value);
            }
            catch (FormatException e)
            {
                throw new InvalidCommandArgumentException(parameter.Name ?? "<unknown>", true);
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
