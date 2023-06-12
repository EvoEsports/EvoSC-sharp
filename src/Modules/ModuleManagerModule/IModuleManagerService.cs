using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Interfaces;

namespace EvoSC.Modules.Official.ModuleManagerModule;

public interface IModuleManagerService
{
    public Task EnableModuleAsync(IModuleLoadContext module);
    public Task DisableModuleAsync(IModuleLoadContext module);
    public Task ListModulesAsync(IPlayer actor);
}
