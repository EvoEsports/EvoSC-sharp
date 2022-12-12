using EvoSC.Modules.Interfaces;

namespace EvoSC.Modules.Models;

public class InternalModuleInfo : IInternalModuleInfo
{
    public required string Name { get; init; }
    public required string Title { get; init; }
    public required string Summary { get; init; }
    public required Version Version { get; init; }
    public required string Author { get; init; }
    public IEnumerable<IModuleDependency> Dependencies { get; init; }
}
