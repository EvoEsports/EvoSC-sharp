using System.Reflection;
using EvoSC.Modules.Extensions;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules;

public class ModuleManager : IModuleManager
{
    private readonly ILogger<ModuleManager> _logger;

    private Dictionary<Guid, IModuleLoadContext> _loadedPlugins = new();

    public ModuleManager(ILogger<ModuleManager> logger)
    {
        _logger = logger;
    }

    
    
    public async Task LoadModulesFromAssembly(Assembly assembly)
    {
        foreach (var asmModule in assembly.Modules)
        {
            foreach (var moduleType in asmModule.GetTypes())
            {
                if (!moduleType.IsEvoScModuleType())
                {
                    continue;
                }

                var moduleAttr = moduleType.GetModuleAttribute();
            }
        }
    }
}
