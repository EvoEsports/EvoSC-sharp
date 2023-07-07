using EvoSC.Common.Application;

namespace EvoSC.Common.Interfaces.Application;

public interface IAsyncActionStartupComponent : IStartupComponent
{
    /// <summary>
    /// The method to call for this action.
    /// </summary>
    public Func<ServicesBuilder, Task> AsyncAction { get; }
}
