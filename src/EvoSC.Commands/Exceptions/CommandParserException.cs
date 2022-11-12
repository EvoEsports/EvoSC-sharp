using System.Runtime.Serialization;

namespace EvoSC.Commands.Exceptions;

/// <summary>
/// General exception for handling errors during parsing of user input.
/// </summary>
public class CommandParserException : CommandException
{
    private readonly bool _intendedCommand;

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

    /// <summary>
    /// Whether the command was intended by the user to be an actual command. Eg. prefixing with "/".
    /// If this is false, the message was either an alias or not supposed to be a command at all.
    /// </summary>
    public bool IntendedCommand => _intendedCommand;
}
