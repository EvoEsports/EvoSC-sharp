using System.Runtime.Serialization;

namespace EvoSC.Common.Exceptions;

public class DuplicateMapException : EvoSCException
{
    public DuplicateMapException()
    {
    }
    
    protected DuplicateMapException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public DuplicateMapException(string? message) : base(message)
    {
    }

    public DuplicateMapException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
