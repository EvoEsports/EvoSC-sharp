using System.Runtime.Serialization;

namespace EvoSC.Commands.Exceptions;

/// <summary>
/// General exception for formatting errors of user input during parsing.
/// </summary>
public class InvalidCommandFormatException : CommandParserException
{
    public InvalidCommandFormatException(bool intendedCommand) : base(intendedCommand)
    {
    }

    protected InvalidCommandFormatException(SerializationInfo info, StreamingContext context, bool intendedCommand) : base(info, context, intendedCommand)
    {
    }

    public InvalidCommandFormatException(string? message, bool intendedCommand) : base(message, intendedCommand)
    {
    }

    public InvalidCommandFormatException(string? message, Exception? innerException, bool intendedCommand) : base(message, innerException, intendedCommand)
    {
    }
}
