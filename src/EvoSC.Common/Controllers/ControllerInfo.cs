namespace EvoSC.Common.Controllers;

public class ControllerInfo
{
    public Type ControllerType { get; init; }
    public Guid ModuleId { get; init; }

    public ControllerInfo(Type type, Guid moduleId)
    {
        ControllerType = type;
        ModuleId = moduleId;
    }
}
