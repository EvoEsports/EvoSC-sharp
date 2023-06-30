using EvoSC.Common.Interfaces.Application;

namespace EvoSC.Common.Application.Models;

public class ActionStartupComponent : IActionStartupComponent
{
    public required string Name { get; init; }
    public required List<string> Dependencies { get; init; }
    public required Action<ServicesBuilder> Action { get; init; }
}
