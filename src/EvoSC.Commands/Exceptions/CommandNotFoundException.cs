namespace EvoSC.Commands.Exceptions;

/// <summary>
/// Thrown when a command that does not exist was attempted retrieval.
/// </summary>
public class CommandNotFoundException : CommandParserException
{
    private readonly string _cmdName;
    
    public CommandNotFoundException(string cmdName, bool intendedCommand) : base(intendedCommand)
    {
        _cmdName = cmdName;
    }

    public override string Message => $"The command '{_cmdName}' was not found.";
}
