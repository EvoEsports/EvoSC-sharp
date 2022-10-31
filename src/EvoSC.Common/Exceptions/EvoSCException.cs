using System.Runtime.Serialization;

namespace EvoSC.Common.Exceptions;

public class EvoSCException : Exception
{
    public EvoSCException()
    {
    }

    protected EvoSCException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public EvoSCException(string? message) : base(message)
    {
    }

    public EvoSCException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
