using System.Runtime.Serialization;
using EvoSC.Common.Exceptions;

namespace EvoSC.Commands.Exceptions;

/// <summary>
/// General exception for errors in the command system.
/// </summary>
public class CommandException : EvoSCException
{
    public CommandException()
    {
    }

    protected CommandException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public CommandException(string? message) : base(message)
    {
    }

    public CommandException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
