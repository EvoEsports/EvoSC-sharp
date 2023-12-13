namespace EvoSC.Commands.Exceptions;

/// <summary>
/// Thrown when the input does not have enough arguments to call a command's handler.
/// </summary>
public class NotEnoughArgumentsException(int requiredArgs, bool intendedCommand) : CommandParserException(intendedCommand)
{
    public override string Message => $"The command requires {requiredArgs} argument(s).";
}
