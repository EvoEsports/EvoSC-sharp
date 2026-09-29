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
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.ModuleManagerModule.Services;

[Service]
public class ModuleManagerService(IContextService context, IModuleManager modules, IChatService chat, Locale locale,
    ILogger<ModuleManagerService> logger) : IModuleManagerService
{
    private readonly dynamic _locale = locale;

    public Task EnableModuleAsync(IModuleLoadContext module)
    {
        string auditComment = _locale.Audit_ModuleEnabled;
        return RunModuleOperationAsync(module, AuditEvents.ModuleEnabled, auditComment,
            () => modules.EnableAsync(module.LoadId),
            () => _locale.PlayerLanguage.ModuleWasEnabled(module.ModuleInfo.Id),
            ex => _locale.PlayerLanguage.FailedEnablingModule(ex.Message));
    }

    public async Task DisableModuleAsync(IModuleLoadContext module)
    {
        var refusal = GetDisableRefusal(module);
        var actor = context.Audit().Actor;

        if (refusal != null)
        {
            string auditComment = _locale.Audit_ModuleDisabled;
            context.Audit()
                .WithEventName(AuditEvents.ModuleDisabled)
                .HavingProperties(new {module.LoadId, module.ModuleInfo})
                .Comment(auditComment)
                .Error();

            if (actor != null)
            {
                await chat.ErrorMessageAsync(_locale.PlayerLanguage.FailedDisablingModule(refusal), actor);
            }
            return;
        }

        string comment = _locale.Audit_ModuleDisabled;
        await RunModuleOperationAsync(module, AuditEvents.ModuleDisabled, comment,
            () => modules.DisableAsync(module.LoadId),
            () => _locale.PlayerLanguage.ModuleWasDisabled(module.ModuleInfo.Id),
            ex => _locale.PlayerLanguage.FailedDisablingModule(ex.Message));
    }

    public Task LoadModuleAsync(string directory)
    {
        string auditComment = _locale.Audit_ModuleLoaded;
        return RunDirectoryOperationAsync(AuditEvents.ModuleLoaded, directory, auditComment,
            moduleInfo => modules.LoadAsync(moduleInfo, install: false),
            id => _locale.PlayerLanguage.ModuleWasLoaded(id),
            ex => _locale.PlayerLanguage.FailedLoadingModule(ex.Message));
    }

    public Task ReloadModuleAsync(IModuleLoadContext module)
    {
        string auditComment = _locale.Audit_ModuleReloaded;
        return RunModuleOperationAsync(module, AuditEvents.ModuleReloaded, auditComment,
            () => modules.ReloadAsync(module.LoadId),
            () => _locale.PlayerLanguage.ModuleWasReloaded(module.ModuleInfo.Id),
            ex => _locale.PlayerLanguage.FailedReloadingModule(ex.Message));
    }

    public Task UnloadModuleAsync(IModuleLoadContext module)
    {
        string auditComment = _locale.Audit_ModuleUnloaded;
        return RunModuleOperationAsync(module, AuditEvents.ModuleUnloaded, auditComment,
            () => modules.UnloadAsync(module.LoadId),
            () => _locale.PlayerLanguage.ModuleWasUnloaded(module.ModuleInfo.Id),
            ex => _locale.PlayerLanguage.FailedUnloadingModule(ex.Message));
    }

    public Task InstallModuleAsync(string directory)
    {
        string auditComment = _locale.Audit_ModuleInstalled;
        return RunDirectoryOperationAsync(AuditEvents.ModuleInstalled, directory, auditComment,
            moduleInfo => modules.LoadAsync(moduleInfo, install: true),
            id => _locale.PlayerLanguage.ModuleWasInstalled(id),
            ex => _locale.PlayerLanguage.FailedInstallingModule(ex.Message));
    }

    public Task UninstallModuleAsync(IModuleLoadContext module)
    {
        string auditComment = _locale.Audit_ModuleUninstalled;
        return RunModuleOperationAsync(module, AuditEvents.ModuleUninstalled, auditComment,
            () => modules.UninstallAsync(module.LoadId),
            () => _locale.PlayerLanguage.ModuleWasUninstalled(module.ModuleInfo.Id),
            ex => _locale.PlayerLanguage.FailedUninstallingModule(ex.Message));
    }

    public async Task ListModulesAsync()
    {
        var actor = context.Audit().Actor;
        await chat.InfoMessageAsync(_locale.PlayerLanguage.GetLoadedModules(), actor);

        foreach (var module in modules.GetLoadedModules().OrderBy(m => m.ModuleInfo.Id, StringComparer.Ordinal))
        {
            var message = new TextFormatter();
            var (text, color) = GetStatusStyle(module.Status);

            message.AddText(text, style => style.WithColor(color).AsBold());
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
    /// Runs an audited module operation: on success the module is reported to the actor, on failure
    /// the audit is marked as an error and the message is reported instead. The failure reason is
    /// logged so a module that fails to toggle can be diagnosed after the fact.
    /// </summary>
    private async Task RunModuleOperationAsync(IModuleLoadContext module, Enum eventName, string comment,
        Func<Task> operation, Func<string> success, Func<Exception, string> failure)
    {
        context.Audit()
            .WithEventName(eventName)
            .HavingProperties(new {module.LoadId, module.ModuleInfo})
            .Comment(comment);

        var actor = context.Audit().Actor;

        try
        {
            await operation();
            context.Audit().Success();
            if (actor != null)
            {
                await chat.SuccessMessageAsync(success(), actor);
            }
        }
        catch (Exception ex)
        {
            context.Audit().Error();
            logger.LogError(ex, "Module operation '{Event}' failed for module '{Module}'", eventName,
                module.ModuleInfo.Id);
            if (actor != null)
            {
                await chat.ErrorMessageAsync(failure(ex), actor);
            }
        }
    }

    /// <summary>
    /// Runs an audited directory operation, reading the module's info up front so the actor is told
    /// which module was affected and a directory that holds no module is refused before anything is
    /// loaded.
    /// </summary>
    private async Task RunDirectoryOperationAsync(Enum eventName, string directory, string comment,
        Func<IExternalModuleInfo, Task> operation, Func<string, string> success, Func<Exception, string> failure)
    {
        context.Audit()
            .WithEventName(eventName)
            .HavingProperties(new {Directory = directory})
            .Comment(comment);

        var actor = context.Audit().Actor;

        try
        {
            var moduleInfo = ModuleInfoUtils.CreateFromDirectory(new DirectoryInfo(directory));
            await operation(moduleInfo);
            context.Audit().Success();
            if (actor != null)
            {
                await chat.SuccessMessageAsync(success(moduleInfo.Id), actor);
            }
        }
        catch (Exception ex)
        {
            context.Audit().Error();
            logger.LogError(ex, "Module operation '{Event}' failed for directory '{Directory}'", eventName, directory);
            if (actor != null)
            {
                await chat.ErrorMessageAsync(failure(ex), actor);
            }
        }
    }

    /// <summary>
    /// Why this module must not be disabled, or null when it may be. A module another enabled module
    /// depends on cannot be disabled: that module would keep running with a dependency that is gone.
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

    private (string Text, Color Color) GetStatusStyle(ModuleStatus status) => status switch
    {
        ModuleStatus.Enabled => (_locale.Status_Enabled, Color.LimeGreen),
        ModuleStatus.Loaded => (_locale.Status_Loaded, Color.Gold),
        ModuleStatus.Disabled => (_locale.Status_Disabled, Color.IndianRed),
        _ => (_locale.Status_Error, Color.Red)
    };
}