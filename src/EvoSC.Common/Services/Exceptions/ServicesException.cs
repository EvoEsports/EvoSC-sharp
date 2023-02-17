using System.Runtime.Serialization;
using EvoSC.Common.Exceptions;

namespace EvoSC.Common.Services.Exceptions;

/// <summary>
/// General exception for errors occuring with module services.
/// </summary>
public class ServicesException : EvoSCException
{
    public ServicesException()
    {
    }

    protected ServicesException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public ServicesException(string? message) : base(message)
    {
    }

    public ServicesException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
