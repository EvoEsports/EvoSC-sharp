using System.Runtime.Serialization;
using EvoSC.Modules.Exceptions;

namespace EvoSC.Modules.Exceptions.ModuleServices;

public class ModuleServicesException : EvoScModuleException
{
    public ModuleServicesException()
    {
    }

    protected ModuleServicesException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public ModuleServicesException(string? message) : base(message)
    {
    }

    public ModuleServicesException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
