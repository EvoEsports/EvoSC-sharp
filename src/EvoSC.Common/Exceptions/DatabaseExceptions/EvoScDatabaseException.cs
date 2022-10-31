using System.Runtime.Serialization;

namespace EvoSC.Common.Exceptions.DatabaseExceptions;

public class EvoScDatabaseException : EvoSCException
{
    public EvoScDatabaseException()
    {
    }

    protected EvoScDatabaseException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public EvoScDatabaseException(string? message) : base(message)
    {
    }

    public EvoScDatabaseException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
