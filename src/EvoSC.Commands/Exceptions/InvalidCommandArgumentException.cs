namespace EvoSC.Commands.Exceptions;

/// <summary>
/// Thrown when the user input has an invalid format for a given command's parameter.
/// </summary>
public class InvalidCommandArgumentException(string name, bool intendedCommand) : InvalidCommandFormatException(intendedCommand)
{
    public override string Message => $"The argument '{name}' is in an invalid format.";
}
