using System.Runtime.Serialization;

namespace EvoSC.Commands.Exceptions;

public class CommandParserException : CommandException
{
    private bool _intendedCommand;

    public CommandParserException( bool intendedCommand)
    {
        _intendedCommand = intendedCommand;
    }
    
    protected CommandParserException(SerializationInfo info, StreamingContext context, bool intendedCommand) : base(info, context)
    {
        _intendedCommand = intendedCommand;
    }

    public CommandParserException(string? message,  bool intendedCommand) : base(message)
    {
        _intendedCommand = intendedCommand;
    }

    public CommandParserException(string? message, Exception? innerException,  bool intendedCommand) : base(message, innerException)
    {
        _intendedCommand = intendedCommand;
    }

    public bool IntendedCommand => _intendedCommand;
}
