using System.Runtime.Serialization;

namespace EvoSC.Modules.Exceptions.ModuleServices;

/// <summary>
/// General exception for errors occuring with module services.
/// </summary>
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
