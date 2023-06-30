using EvoSC.Common.Application;

namespace EvoSC.Common.Interfaces.Application;

public interface IAsyncActionStartupComponent : IStartupComponent
{
    public Func<ServicesBuilder, Task> AsyncAction { get; }
}
