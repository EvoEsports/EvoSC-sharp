using EvoSC.Common.Application;

namespace EvoSC.Common.Interfaces.Application;

public interface IServiceStartupComponent : IStartupComponent
{
    /// <summary>
    /// The method to call to configure the services.
    /// </summary>
    public Action<ServicesBuilder> ConfigAction { get; }
}
