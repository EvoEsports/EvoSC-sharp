namespace EvoSC.Commands.Exceptions;

public class InvalidCommandArgumentException : InvalidCommandFormatException
{
    private readonly string _name;
    
    public InvalidCommandArgumentException(string name, bool intendedCommand) : base(intendedCommand)
    {
        _name = name;
    }

    public override string Message => $"The argument '{_name}' is in an invalid format.";
}
