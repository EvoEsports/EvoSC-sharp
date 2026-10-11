using EvoSC.Modules.Interfaces;

namespace EvoSC.Modules.Models;

/// <summary>
/// Information about a module discovered from a directory on disk, before or while it is being
/// loaded. This is the only representation of a module info outside of a loaded
/// <see cref="IModuleLoadContext"/>'s <see cref="IModuleLoadContext.ModuleInfo"/>.
/// </summary>
public class ExternalModuleInfo : IModuleInfo
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string Summary { get; init; }
    public required Version Version { get; init; }
    public required string Author { get; init; }
    public required IEnumerable<IModuleDependency> Dependencies { get; init; }

    /// <summary>
    /// The directory which this module resides in.
    /// </summary>
    public required DirectoryInfo Directory { get; init; }

    /// <summary>
    /// All files related to this module.
    /// </summary>
    public required IEnumerable<IModuleFile> ModuleFiles { get; init; }

    /// <summary>
    /// All assembly (.dll) files for this module.
    /// </summary>
    public IEnumerable<IModuleFile> AssemblyFiles => ModuleFiles.Where(f => f.IsAssembly);

    /// <summary>
    /// Whether this module is part of the application itself. A module declares this with
    /// [Module(IsInternal = true)], and the application sets it for the modules it registers as
    /// internal. An internal module is never unloaded or reloaded at run time. Settable here,
    /// unlike on <see cref="IModuleInfo"/>: a module's internal status is only ever decided while
    /// it is still being loaded, by <see cref="IModuleManager"/> itself.
    /// </summary>
    public bool IsInternal { get; set; }
}
