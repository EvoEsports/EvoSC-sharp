using EvoSC.Modules.Models;

namespace EvoSC.Modules.Interfaces;

public interface IExternalModuleInfo : IModuleInfo
{
    bool IModuleInfo.IsInternal => false;

    public DirectoryInfo Directory { get; }
    public IEnumerable<IModuleFile> ModuleFiles { get; }
    public IEnumerable<IModuleFile> AssemblyFiles => ModuleFiles.Where(f => f.IsAssembly);
}
