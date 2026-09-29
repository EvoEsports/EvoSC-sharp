using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Official.ModuleManagerModule.Interfaces;

namespace EvoSC.Modules.Official.ModuleManagerModule.Controllers;

[Controller]
public class ModuleCommandsController(IModuleManagerService moduleManagerService) : EvoScController<ICommandInteractionContext>
{
    [ChatCommand("enablemodule", "[Command.EnableModule]", ModuleManagerPermissions.ActivateModule)]
    public Task EnableModuleAsync(IModuleLoadContext module) => moduleManagerService.EnableModuleAsync(module);

    [ChatCommand("disablemodule", "[Command.DisableModule]", ModuleManagerPermissions.ActivateModule)]
    public Task DisableModuleAsync(IModuleLoadContext module) => moduleManagerService.DisableModuleAsync(module);

    [ChatCommand("modules", "[Command.Modules]")]
    public Task ListModulesAsync() => moduleManagerService.ListModulesAsync();

    [ChatCommand("reloadmodule", "[Command.ReloadModule]", ModuleManagerPermissions.ConfigureModules)]
    public Task ReloadModuleAsync(IModuleLoadContext module) => moduleManagerService.ReloadModuleAsync(module);

    [ChatCommand("loadmodule", "[Command.LoadModule]", ModuleManagerPermissions.ConfigureModules)]
    public Task LoadModuleAsync(string directory) => moduleManagerService.LoadModuleAsync(directory);

    [ChatCommand("unloadmodule", "[Command.UnloadModule]", ModuleManagerPermissions.ConfigureModules)]
    public Task UnloadModuleAsync(IModuleLoadContext module) => moduleManagerService.UnloadModuleAsync(module);

    [ChatCommand("installmodule", "[Command.InstallModule]", ModuleManagerPermissions.InstallModule)]
    public Task InstallModuleAsync(string directory) => moduleManagerService.InstallModuleAsync(directory);

    [ChatCommand("uninstallmodule", "[Command.UninstallModule]", ModuleManagerPermissions.InstallModule)]
    public Task UninstallModuleAsync(IModuleLoadContext module) => moduleManagerService.UninstallModuleAsync(module);
}
