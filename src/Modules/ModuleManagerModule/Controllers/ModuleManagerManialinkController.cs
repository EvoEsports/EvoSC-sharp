using EvoSC.Common.Controllers.Attributes;
using EvoSC.Manialinks;
using EvoSC.Manialinks.Attributes;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Official.ModuleManagerModule.Interfaces;

namespace EvoSC.Modules.Official.ModuleManagerModule.Controllers;

[Controller]
public class ModuleManagerManialinkController(IModuleManagerService moduleManagerService, IModuleManager modules) : ManialinkController
{
    [ManialinkRoute(Permission = ModuleManagerPermissions.ActivateModule)]
    public Task EnableModuleAsync(string id) => RunAndRefreshAsync(id, moduleManagerService.EnableModuleAsync);

    [ManialinkRoute(Permission = ModuleManagerPermissions.ActivateModule)]
    public Task DisableModuleAsync(string id) => RunAndRefreshAsync(id, moduleManagerService.DisableModuleAsync);

    [ManialinkRoute(Permission = ModuleManagerPermissions.ConfigureModules)]
    public Task ReloadModuleAsync(string id) => RunAndRefreshAsync(id, moduleManagerService.ReloadModuleAsync);

    [ManialinkRoute(Permission = ModuleManagerPermissions.ConfigureModules)]
    public Task UnloadModuleAsync(string id) => RunAndRefreshAsync(id, moduleManagerService.UnloadModuleAsync);

    [ManialinkRoute(Permission = ModuleManagerPermissions.InstallModule)]
    public Task UninstallModuleAsync(string id) => RunAndRefreshAsync(id, moduleManagerService.UninstallModuleAsync);

    /// <summary>
    /// Runs a module operation by id and re-sends the window afterward, so the player sees the
    /// result of their click without having to reopen it. The window may have gone stale between
    /// being shown and a button being clicked (the module was unloaded by someone else in the
    /// meantime); that is reported like any other failed operation rather than throwing.
    /// </summary>
    private async Task RunAndRefreshAsync(string id, Func<IModuleLoadContext, Task> operation)
    {
        var module = modules.GetLoadedModules().FirstOrDefault(m => m.ModuleInfo.Id == id);

        if (module != null)
        {
            await operation(module);
        }

        await moduleManagerService.ShowManagerWindowAsync(Context.Player);
    }
}
