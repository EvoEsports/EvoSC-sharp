using System.Runtime.Serialization;
using EvoSC.Common.Exceptions;

namespace EvoSC.Modules.Exceptions;

/// <summary>
/// General exception for errors occuring with modules.
/// </summary>
public class EvoScModuleException : EvoSCException
{
    public EvoScModuleException()
    {
    }

    protected EvoScModuleException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public EvoScModuleException(string? message) : base(message)
    {
    }

    public EvoScModuleException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
