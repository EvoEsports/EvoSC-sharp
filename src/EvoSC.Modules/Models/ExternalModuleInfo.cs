using EvoSC.Modules.Interfaces;

namespace EvoSC.Modules.Models;

public class ExternalModuleInfo : IExternalModuleInfo
{
    public required string Name { get; init; }
    public required string Title { get; init; }
    public required string Summary { get; init; }
    public required Version Version { get; init; }
    public required string Author { get; init; }
    public required IEnumerable<IModuleDependency> Dependencies { get; init; }
    public required DirectoryInfo Directory { get; init; }
    public required IEnumerable<IModuleFile> ModuleFiles { get; init; }
}
