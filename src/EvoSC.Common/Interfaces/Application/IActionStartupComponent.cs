using EvoSC.Common.Application;

namespace EvoSC.Common.Interfaces.Application;

public interface IActionStartupComponent : IStartupComponent
{
    public Action<ServicesBuilder> Action { get; }
}
