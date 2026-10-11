namespace EvoSC.Modules.Interfaces;

public interface IExternalModuleInfo : IModuleInfo
{
    /// <summary>
    /// Whether this module is internal or not. Settable here, unlike on <see cref="IModuleInfo"/>:
    /// a module's internal status is only ever decided while it is still being loaded, by
    /// <see cref="IModuleManager"/> itself.
    /// </summary>
    public new bool IsInternal { get; set; }

    /// <summary>
    /// The directory which this module resides in.
    /// </summary>
    public DirectoryInfo Directory { get; }
    
    /// <summary>
    /// All files related to this module.
    /// </summary>
    public IEnumerable<IModuleFile> ModuleFiles { get; }
    
    /// <summary>
    /// All assembly (.dll) files for this module.
    /// </summary>
    public IEnumerable<IModuleFile> AssemblyFiles => ModuleFiles.Where(f => f.IsAssembly);
}
