namespace EvoSC.Commands.Exceptions;

/// <summary>
/// Thrown when a command that does not exist was attempted retrieval.
/// </summary>
public class CommandNotFoundException(string cmdName, bool intendedCommand) : CommandParserException(intendedCommand)
{
    public override string Message => $"The command '{cmdName}' was not found.";
}
