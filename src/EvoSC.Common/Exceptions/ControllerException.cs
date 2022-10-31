using System.Runtime.Serialization;

namespace EvoSC.Common.Exceptions;

public class ControllerException : EvoSCException
{
    public ControllerException()
    {
    }

    protected ControllerException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public ControllerException(string? message) : base(message)
    {
    }

    public ControllerException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
