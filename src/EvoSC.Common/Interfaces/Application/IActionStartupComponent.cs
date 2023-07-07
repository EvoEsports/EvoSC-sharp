using EvoSC.Common.Application;

namespace EvoSC.Common.Interfaces.Application;

public interface IActionStartupComponent : IStartupComponent
{
    /// <summary>
    /// The method to call for this action.
    /// </summary>
    public Action<ServicesBuilder> Action { get; }
}
