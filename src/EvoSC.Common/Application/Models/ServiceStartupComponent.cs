using EvoSC.Common.Interfaces.Application;

namespace EvoSC.Common.Application.Models;

public class ServiceStartupComponent : IServiceStartupComponent
{
    public required string Name { get; init; }
    public required List<string> Dependencies { get; init; }
    public required Action<ServicesBuilder> ConfigAction { get; init; }
}
