using System.Reflection;
using EvoSC.Modules.Attributes;
using EvoSC.Modules.Exceptions;
using EvoSC.Modules.Extensions;

namespace EvoSC.Modules;

public class ModuleLoader
{
    private ModuleAttribute _moduleInfo;
    private Type _moduleType;
    
    public ModuleLoader(Type moduleType)
    {
        if (!moduleType.IsEvoScModuleType())
        {
            throw new EvoScNotModuleException();
        }
        
        _moduleType = moduleType;
        _moduleInfo = moduleType.GetModuleAttribute();
    }

    public static bool IsModule(Type type) =>
        type.GetCustomAttribute<ModuleAttribute>() != null;
}
