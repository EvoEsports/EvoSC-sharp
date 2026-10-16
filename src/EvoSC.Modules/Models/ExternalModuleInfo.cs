using EvoSC.Modules.Interfaces;

namespace EvoSC.Modules.Models;

public class ExternalModuleInfo : IExternalModuleInfo
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string Summary { get; init; }
    public required Version Version { get; init; }
    public required string Author { get; init; }
    public required IEnumerable<IModuleDependency> Dependencies { get; init; }
    public required DirectoryInfo Directory { get; init; }
    public required IEnumerable<IModuleFile> ModuleFiles { get; init; }

    /// <summary>
    /// Whether this module is part of the application itself. A module declares this with
    /// [Module(IsInternal = true)], and the application sets it for the modules it registers as
    /// internal. An internal module is never unloaded or reloaded at run time.
    /// </summary>
    public bool IsInternal { get; set; }
}
