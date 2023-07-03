namespace EvoSC.CLI.Exceptions;

public class InvalidCommandClassFormatException : EvoScCliException
{
    public InvalidCommandClassFormatException(string? message) : base(message)
    {
    }
}
