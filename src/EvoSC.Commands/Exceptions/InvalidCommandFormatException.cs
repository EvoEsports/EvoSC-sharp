using System.Runtime.Serialization;

namespace EvoSC.Commands.Exceptions;

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
