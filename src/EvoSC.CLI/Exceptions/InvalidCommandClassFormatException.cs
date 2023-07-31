namespace EvoSC.CLI.Exceptions;

/// <summary>
/// Exception which occurs when a command class is not defined correctly.
/// </summary>
public class InvalidCommandClassFormatException : EvoScCliException
{
    public InvalidCommandClassFormatException(string? message) : base(message)
    {
    }
}
