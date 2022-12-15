using EvoSC.Modules.Models;

namespace EvoSC.Modules.Interfaces;

public interface IExternalModuleInfo : IModuleInfo
{
    bool IModuleInfo.IsInternal => false;

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
