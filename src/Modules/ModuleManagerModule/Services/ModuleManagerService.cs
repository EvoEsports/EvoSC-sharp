using System.Drawing;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Util.TextFormatting;
using EvoSC.Modules;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Official.ModuleManagerModule.Events;
using EvoSC.Modules.Official.ModuleManagerModule.Interfaces;
using EvoSC.Modules.Util;

namespace EvoSC.Modules.Official.ModuleManagerModule.Services;


[Service]
public class ModuleManagerService(IContextService context, IModuleManager modules, IChatService chat, Locale locale)
    : IModuleManagerService
{
    private readonly dynamic _locale = locale;

    public async Task EnableModuleAsync(IModuleLoadContext module)
    {
        context.Audit()
            .WithEventName(AuditEvents.ModuleEnabled)
            .HavingProperties(new {module.LoadId, module.ModuleInfo})
            .Comment(_locale.Audit_ModuleEnabled);

        var actor = context.Audit().Actor;
        
        try
        {
            await modules.EnableAsync(module.LoadId);
            context.Audit().Success();
            
            if (actor != null)
            {
                await chat.SuccessMessageAsync(_locale.PlayerLanguage.ModuleWasEnabled(module.ModuleInfo.Id), actor);
            }
        }
        catch (Exception ex)
        {
            context.Audit().Error();
            
            if (actor != null)
            {
                await chat.ErrorMessageAsync(_locale.PlayerLanguage.FailedEnablingModule(ex.Message), actor);
            }
        }
    }

    public async Task DisableModuleAsync(IModuleLoadContext module)
    {
        context.Audit()
            .WithEventName(AuditEvents.ModuleDisabled)
            .Comment(_locale.Audit_ModuleDisabled);

        var actor = context.Audit().Actor;
        var refusal = GetDisableRefusal(module);

        if (refusal != null)
        {
            context.Audit()
                .HavingProperties(new {module.LoadId, module.ModuleInfo, Reason = refusal})
                .Error();

            if (actor != null)
            {
                await chat.ErrorMessageAsync(_locale.PlayerLanguage.FailedDisablingModule(refusal), actor);
            }

            return;
        }

        context.Audit().HavingProperties(new {module.LoadId, module.ModuleInfo});

        try
        {
            await modules.DisableAsync(module.LoadId);
            context.Audit().Success();
            
            if (actor != null)
            {
                await chat.SuccessMessageAsync(_locale.PlayerLanguage.ModuleWasDisabled(module.ModuleInfo.Id), actor);
            }
        }
        catch (Exception ex)
        {
            context.Audit().Error();
            
            if (actor != null)
            {
                await chat.ErrorMessageAsync(_locale.PlayerLanguage.FailedDisablingModule(ex.Message), actor);
            }
        }
    }

    public async Task LoadModuleAsync(string directory)
    {
        context.Audit()
            .WithEventName(AuditEvents.ModuleLoaded)
            .HavingProperties(new {Directory = directory})
            .Comment(_locale.Audit_ModuleLoaded);

        var actor = context.Audit().Actor;

        try
        {
            // Read the info first so the administrator is told which module was loaded, and why
            // a directory that holds no module is refused before anything is loaded. Loading a
            // module it does not run its installation: that is what install is for.
            var moduleInfo = ModuleInfoUtils.CreateFromDirectory(new DirectoryInfo(directory));
            await modules.LoadAsync(moduleInfo, install: false);
            context.Audit().Success();

            if (actor != null)
            {
                await chat.SuccessMessageAsync(_locale.PlayerLanguage.ModuleWasLoaded(moduleInfo.Id), actor);
            }
        }
        catch (Exception ex)
        {
            context.Audit().Error();

            if (actor != null)
            {
                await chat.ErrorMessageAsync(_locale.PlayerLanguage.FailedLoadingModule(ex.Message), actor);
            }
        }
    }

    public async Task ReloadModuleAsync(IModuleLoadContext module)
    {
        context.Audit()
            .WithEventName(AuditEvents.ModuleReloaded)
            .HavingProperties(new {module.LoadId, module.ModuleInfo})
            .Comment(_locale.Audit_ModuleReloaded);

        var actor = context.Audit().Actor;

        try
        {
            await modules.ReloadAsync(module.LoadId);
            context.Audit().Success();

            if (actor != null)
            {
                await chat.SuccessMessageAsync(_locale.PlayerLanguage.ModuleWasReloaded(module.ModuleInfo.Id), actor);
            }
        }
        catch (Exception ex)
        {
            context.Audit().Error();

            if (actor != null)
            {
                await chat.ErrorMessageAsync(_locale.PlayerLanguage.FailedReloadingModule(ex.Message), actor);
            }
        }
    }

    public async Task UnloadModuleAsync(IModuleLoadContext module)
    {
        context.Audit()
            .WithEventName(AuditEvents.ModuleUnloaded)
            .HavingProperties(new {module.LoadId, module.ModuleInfo})
            .Comment(_locale.Audit_ModuleUnloaded);

        var actor = context.Audit().Actor;

        try
        {
            await modules.UnloadAsync(module.LoadId);
            context.Audit().Success();

            if (actor != null)
            {
                await chat.SuccessMessageAsync(_locale.PlayerLanguage.ModuleWasUnloaded(module.ModuleInfo.Id), actor);
            }
        }
        catch (Exception ex)
        {
            context.Audit().Error();

            if (actor != null)
            {
                await chat.ErrorMessageAsync(_locale.PlayerLanguage.FailedUnloadingModule(ex.Message), actor);
            }
        }
    }

    public async Task InstallModuleAsync(string directory)
    {
        context.Audit()
            .WithEventName(AuditEvents.ModuleInstalled)
            .HavingProperties(new {Directory = directory})
            .Comment(_locale.Audit_ModuleInstalled);

        var actor = context.Audit().Actor;

        try
        {
            // Read the info first so the administrator is told which module was installed, and why
            // a directory that holds no module is refused before anything is loaded.
            var moduleInfo = ModuleInfoUtils.CreateFromDirectory(new DirectoryInfo(directory));
            await modules.LoadAsync(directory);
            context.Audit().Success();

            if (actor != null)
            {
                await chat.SuccessMessageAsync(_locale.PlayerLanguage.ModuleWasInstalled(moduleInfo.Id), actor);
            }
        }
        catch (Exception ex)
        {
            context.Audit().Error();

            if (actor != null)
            {
                await chat.ErrorMessageAsync(_locale.PlayerLanguage.FailedInstallingModule(ex.Message), actor);
            }
        }
    }

    public async Task UninstallModuleAsync(IModuleLoadContext module)
    {
        context.Audit()
            .WithEventName(AuditEvents.ModuleUninstalled)
            .HavingProperties(new {module.LoadId, module.ModuleInfo})
            .Comment(_locale.Audit_ModuleUninstalled);

        var actor = context.Audit().Actor;

        try
        {
            await modules.UninstallAsync(module.LoadId);
            context.Audit().Success();

            if (actor != null)
            {
                await chat.SuccessMessageAsync(_locale.PlayerLanguage.ModuleWasUninstalled(module.ModuleInfo.Id), actor);
            }
        }
        catch (Exception ex)
        {
            context.Audit().Error();

            if (actor != null)
            {
                await chat.ErrorMessageAsync(_locale.PlayerLanguage.FailedUninstallingModule(ex.Message), actor);
            }
        }
    }

    public async Task ListModulesAsync(IPlayer actor)
    {
        await chat.InfoMessageAsync(_locale.PlayerLanguage.GetLoadedModules(), actor);

        foreach (var module in modules.GetLoadedModules().OrderBy(m => m.ModuleInfo.Id, StringComparer.Ordinal))
        {
            var message = new TextFormatter();

            message.AddText(GetStatusText(module.Status), style => style
                .WithColor(GetStatusColor(module.Status))
                .AsBold()
            );

            message.AddText(" ");

            message.AddText(module.ModuleInfo.Id);

            message.AddText($" ({module.ModuleInfo.Name})");
            message.AddText($" v{module.ModuleInfo.Version}");

            if (module.ModuleInfo.IsInternal)
            {
                message.AddText($" ({_locale.PlayerLanguage.InternalModule})", style => style.WithColor(Color.Gray));
            }

            await chat.InfoMessageAsync(message, actor);
        }
    }

    /// <summary>
    /// Why this module must not be disabled, or null when it may be. A module another enabled
    /// module depends on cannot be disabled either: that module would keep running with a dependency
    /// that is no longer there.
    /// </summary>
    private string? GetDisableRefusal(IModuleLoadContext module)
    {
        var dependents = modules.GetLoadedModules()
            .Where(m => m.LoadId != module.LoadId && m.IsEnabled && m.LoadedDependencies.Contains(module.LoadId))
            .Select(m => m.ModuleInfo.Id)
            .OrderBy(id => id, StringComparer.Ordinal)
            .ToArray();

        return dependents.Length > 0
            ? _locale.CannotDisableModuleWithDependents(string.Join(", ", dependents))
            : null;
    }

    private string GetStatusText(ModuleStatus status) => status switch
    {
        ModuleStatus.Enabled => _locale.Status_Enabled,
        ModuleStatus.Loaded => _locale.Status_Loaded,
        ModuleStatus.Disabled => _locale.Status_Disabled,
        _ => _locale.Status_Error
    };

    private static Color GetStatusColor(ModuleStatus status) => status switch
    {
        ModuleStatus.Enabled => Color.LimeGreen,
        ModuleStatus.Loaded => Color.Gold,
        ModuleStatus.Disabled => Color.IndianRed,
        _ => Color.Red
    };
}
