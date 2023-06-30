using EvoSC.Common.Application;

namespace EvoSC.Common.Interfaces.Application;

public interface IServiceStartupComponent : IStartupComponent
{
    public Action<ServicesBuilder> ConfigAction { get; }
}
