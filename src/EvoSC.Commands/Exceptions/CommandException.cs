using System.Runtime.Serialization;
using EvoSC.Common.Exceptions;

namespace EvoSC.Commands.Exceptions;

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
