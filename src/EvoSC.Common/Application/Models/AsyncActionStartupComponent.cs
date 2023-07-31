using EvoSC.Common.Interfaces.Application;

namespace EvoSC.Common.Application.Models;

public class AsyncActionStartupComponent : IAsyncActionStartupComponent
{
    public required string Name { get; init; }
    public required List<string> Dependencies { get; init; }
    public required Func<ServicesBuilder, Task> AsyncAction { get; init; }
}
