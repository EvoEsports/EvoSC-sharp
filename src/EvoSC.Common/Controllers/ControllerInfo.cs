using SimpleInjector;

namespace EvoSC.Common.Controllers;

public class ControllerInfo
{
    public Type ControllerType { get; init; }
    public Guid ModuleId { get; init; }
    public Container Services { get; init; }

    public ControllerInfo(Type type, Guid moduleId, Container services)
    {
        ControllerType = type;
        ModuleId = moduleId;
        Services = services;
    }
}
