namespace EvoSC.Commands.Exceptions;

/// <summary>
/// Thrown when the user input has an invalid format for a given command's parameter.
/// </summary>
public class InvalidCommandArgumentException : InvalidCommandFormatException
{
    private readonly string _name;
    
    public InvalidCommandArgumentException(string name, bool intendedCommand) : base(intendedCommand)
    {
        _name = name;
    }

    public override string Message => $"The argument '{_name}' is in an invalid format.";
}
