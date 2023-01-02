using System.Reflection;

namespace EvoSC.Modules.Interfaces;

public interface IInternalModuleInfo : IModuleInfo
{
    bool IModuleInfo.IsInternal => true;
    
    /// <summary>
    /// The assembly this module is attached to.
    /// </summary>
    public Assembly Assembly { get; }
}
