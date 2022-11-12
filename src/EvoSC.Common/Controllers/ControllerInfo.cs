using SimpleInjector;

namespace EvoSC.Common.Controllers;

public class ControllerInfo
{
    public required Type ControllerType { get; init; }
    public required Guid ModuleId { get; init; }
    public required Container Services { get; init; }
}
